using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using CEN4090L_Project.Models;
using CollegeCompanion.Library.Services;

namespace CEN4090L_Project.ViewModels
{
    public class ExpensePageViewModel : INotifyPropertyChanged
    {
        private DatabaseService _db => DatabaseService.Current;

        private string? _description;
        private string? _amount;
        private DateTime _date = DateTime.Now;
        private BudgetCategory? _selectedCategory;
        private bool _needsSelected;
        private bool _wantsSelected;
        private bool _savingsSelected;

        public string? Description
        {
            get => _description;
            set => Set(ref _description, value);
        }

        public string? Amount
        {
            get => _amount;
            set => Set(ref _amount, value);
        }

        public DateTime Date
        {
            get => _date;
            set => Set(ref _date, value);
        }

        public BudgetCategory? SelectedCategory
        {
            get => _selectedCategory;
            set => Set(ref _selectedCategory, value);
        }

        public bool NeedsSelected
        {
            get => _needsSelected;
            set => Set(ref _needsSelected, value);
        }

        public bool WantsSelected
        {
            get => _wantsSelected;
            set => Set(ref _wantsSelected, value);
        }

        public bool SavingsSelected
        {
            get => _savingsSelected;
            set => Set(ref _savingsSelected, value);
        }

        public ICommand SelectCategoryCommand { get; }
        public ICommand SaveExpenseCommand { get; }

        public ExpensePageViewModel()
        {
            SelectCategoryCommand = new Command<string>(OnSelectCategory);
            SaveExpenseCommand = new Command(async () => await OnSaveExpense());
        }

        private void OnSelectCategory(string category)
        {
            NeedsSelected = false;
            WantsSelected = false;
            SavingsSelected = false;

            switch (category)
            {
                case "Needs":
                    NeedsSelected = true;
                    SelectedCategory = BudgetCategory.Needs;
                    break;
                case "Wants":
                    WantsSelected = true;
                    SelectedCategory = BudgetCategory.Wants;
                    break;
                case "Savings":
                    SavingsSelected = true;
                    SelectedCategory = BudgetCategory.Savings;
                    break;
            }
        }

        private async Task OnSaveExpense()
        {
            // Check if user is logged in
            if (!_db.IsUserLoggedIn())
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Please log in first.", "OK");
                return;
            }

            // Validation
            if (string.IsNullOrWhiteSpace(Description))
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Please enter a description.", "OK");
                return;
            }

            if (string.IsNullOrWhiteSpace(Amount) || !decimal.TryParse(Amount, out decimal amountValue) || amountValue <= 0)
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Please enter a valid amount.", "OK");
                return;
            }

            if (!SelectedCategory.HasValue)
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Please select a category.", "OK");
                return;
            }

            try
            {
                // Convert BudgetCategory enum to string
                string categoryString = SelectedCategory.Value.ToString();

                // Save to database
                bool success = _db.AddExpense(Description, amountValue, categoryString);

                if (success)
                {
                    Console.WriteLine($"[EXPENSE VM] Expense saved: {Description} - ${amountValue} - {categoryString}");

                    // Notify DashboardViewModel to refresh if it exists
                    if (DashboardViewModel.Instance != null)
                    {
                        DashboardViewModel.Instance.LoadExpenses();
                    }

                    await Application.Current.MainPage.DisplayAlert("Success",
                        "Expense added successfully!",
                        "OK");

                    // Clear form
                    Description = string.Empty;
                    Amount = string.Empty;
                    Date = DateTime.Now;
                    NeedsSelected = false;
                    WantsSelected = false;
                    SavingsSelected = false;
                    SelectedCategory = null;

                    await Application.Current.MainPage.Navigation.PopAsync();
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Error",
                        "Failed to save expense. Please try again.",
                        "OK");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[EXPENSE VM] Error: {ex.Message}");
                await Application.Current.MainPage.DisplayAlert("Error",
                    $"Failed to save expense: {ex.Message}",
                    "OK");
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected bool Set<T>(ref T field, T value, [CallerMemberName] string? name = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(name);
            return true;
        }

        protected void OnPropertyChanged([CallerMemberName] string? name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}