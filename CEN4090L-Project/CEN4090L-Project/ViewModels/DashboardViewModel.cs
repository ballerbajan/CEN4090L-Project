using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using CEN4090L_Project.Views;
using CEN4090L_Project.Models;
using Microsoft.Maui.Controls;
using System.Collections.Specialized;

namespace CEN4090L_Project.ViewModels
{
    public class DashboardViewModel : INotifyPropertyChanged
    {
        private static DashboardViewModel _instance;
        public static DashboardViewModel Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new DashboardViewModel();
                return _instance;
            }
        }

        // ----- backing fields -----
        private decimal _income = 0m;
        private decimal _plannedSavings = 0m;
        private ObservableCollection<Expense> _recent = new();

        // 50/30/20 allocations
        public decimal Income
        {
            get => _income;
            set { if (Set(ref _income, value)) Recompute(); }
        }

        public decimal PlannedSavings
        {
            get => _plannedSavings;
            set { Set(ref _plannedSavings, value); }
        }

        public ObservableCollection<Expense> RecentExpenses
        {
            get => _recent;
            set
            {
                // Unsubscribe from old collection
                if (_recent != null)
                {
                    _recent.CollectionChanged -= RecentExpenses_CollectionChanged;
                }

                if (Set(ref _recent, value))
                {
                    // Subscribe to new collection
                    if (_recent != null)
                    {
                        _recent.CollectionChanged += RecentExpenses_CollectionChanged;
                    }
                    Recompute();
                }
            }
        }

        // Handle collection changes (add/remove items)
        private void RecentExpenses_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            Recompute();
        }

        // computed totals
        public decimal TotalNeeds => RecentExpenses?.Where(e => e.Category == BudgetCategory.Needs).Sum(e => e.Amount ?? 0m) ?? 0m;
        public decimal TotalWants => RecentExpenses?.Where(e => e.Category == BudgetCategory.Wants).Sum(e => e.Amount ?? 0m) ?? 0m;
        public decimal TotalSavingsSpent => RecentExpenses?.Where(e => e.Category == BudgetCategory.Savings).Sum(e => e.Amount ?? 0m) ?? 0m;
        public decimal TotalExpenses => TotalNeeds + TotalWants + TotalSavingsSpent;

        // allocation targets
        public decimal NeedsBudget => Math.Round(Income * 0.50m, 2);
        public decimal WantsBudget => Math.Round(Income * 0.30m, 2);
        public decimal SavingsBudget => Math.Round(Income * 0.20m, 2);

        // progress 0..1
        public double NeedsProgress => (double)Math.Clamp((NeedsBudget == 0 ? 0m : TotalNeeds / NeedsBudget), 0, 1);
        public double WantsProgress => (double)Math.Clamp((WantsBudget == 0 ? 0m : TotalWants / WantsBudget), 0, 1);
        public double SavingsProgress => (double)Math.Clamp((SavingsBudget == 0 ? 0m : TotalSavingsSpent / SavingsBudget), 0, 1);

        // display strings
        public string NeedsSpentText => $"Spent {TotalNeeds:C} / {NeedsBudget:C}";
        public string WantsSpentText => $"Spent {TotalWants:C} / {WantsBudget:C}";
        public string SavingsSpentText => $"Saved {TotalSavingsSpent:C} / {SavingsBudget:C}";

        public string NeedsRemainingText => $"{(NeedsBudget - TotalNeeds):C} left";
        public string WantsRemainingText => $"{(WantsBudget - TotalWants):C} left";
        public string SavingsRemainingText => $"{(SavingsBudget - TotalSavingsSpent):C} left";

        public string RemainingThisMonthText => $"Remaining: {(Income - TotalExpenses):C}";

        // ----- commands -----
        public Command EditBudgetCommand { get; }
        public Command AddExpenseCommand { get; }
        public Command<Expense> DeleteExpenseCommand { get; }

        private DashboardViewModel()
        {
            // Initialize with empty collection
            RecentExpenses = new ObservableCollection<Expense>();

            // Subscribe to collection changes
            RecentExpenses.CollectionChanged += RecentExpenses_CollectionChanged;

            EditBudgetCommand = new Command(OnEditBudget);
            AddExpenseCommand = new Command(OnAddExpense);
            DeleteExpenseCommand = new Command<Expense>(OnDeleteExpense);
        }

        private async void OnEditBudget()
        {
            string result = await Application.Current.MainPage.DisplayPromptAsync(
                "Add Income",
                "Enter amount:",
                accept: "Save",
                cancel: "Cancel",
                placeholder: Income.ToString("F2"),
                keyboard: Keyboard.Numeric,
                maxLength: 10
            );

            if (!string.IsNullOrEmpty(result) && decimal.TryParse(result, out decimal amount))
                Income = amount;
        }

        private async void OnAddExpense()
        {
            await Application.Current.MainPage.Navigation.PushAsync(new AddExpensePage());
        }

        private async void OnDeleteExpense(Expense expense)
        {
            if (expense == null) return;

            bool confirm = await Application.Current.MainPage.DisplayAlert(
                "Delete Expense",
                $"Are you sure you want to delete '{expense.Title}' ({expense.Amount:C})?",
                "Delete",
                "Cancel"
            );

            if (!confirm) return;

            try
            {
                RecentExpenses.Remove(expense);
                // CollectionChanged event will automatically call Recompute()

                await Application.Current.MainPage.DisplayAlert(
                    "Success",
                    "Expense deleted successfully",
                    "OK"
                );
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    $"Failed to delete expense: {ex.Message}",
                    "OK"
                );
            }
        }
        // Quick Stats Properties
        public int DaysRemainingInMonth
        {
            get
            {
                var today = DateTime.Today;
                var lastDay = new DateTime(today.Year, today.Month, DateTime.DaysInMonth(today.Year, today.Month));
                return (lastDay - today).Days + 1; // +1 to include today
            }
        }

        public decimal DailyAverageSpending
        {
            get
            {
                var today = DateTime.Today;
                var firstDay = new DateTime(today.Year, today.Month, 1);
                int daysElapsed = (today - firstDay).Days + 1; // +1 to include today

                if (daysElapsed == 0) return 0m;
                return TotalExpenses / daysElapsed;
            }
        }

        public string TopSpendingCategory
        {
            get
            {
                if (TotalExpenses == 0) return "None";

                var maxCategory = new[]
                {
            new { Name = "Needs", Amount = TotalNeeds },
            new { Name = "Wants", Amount = TotalWants },
            new { Name = "Savings", Amount = TotalSavingsSpent }
        }
                .OrderByDescending(c => c.Amount)
                .FirstOrDefault();

                return maxCategory?.Name ?? "None";
            }
        }

        public string BudgetStatus
        {
            get
            {
                if (Income == 0) return "Not Set";

                decimal spendingRate = TotalExpenses / Income;

                if (spendingRate <= 0.75m) return "On Track ✓";
                if (spendingRate <= 0.95m) return "Watch ⚠️";
                return "Over Budget ⚠️";
            }
        }

        public Color BudgetStatusColor
        {
            get
            {
                if (Income == 0) return Color.FromArgb("#6B7280"); // Gray

                decimal spendingRate = TotalExpenses / Income;

                if (spendingRate <= 0.75m) return Color.FromArgb("#10B981"); // Green
                if (spendingRate <= 0.95m) return Color.FromArgb("#F59E0B"); // Orange
                return Color.FromArgb("#EF4444"); // Red
            }
        }

        // recompute when income/expenses change
        private void Recompute()
        {
            OnPropertyChanged(nameof(TotalNeeds));
            OnPropertyChanged(nameof(TotalWants));
            OnPropertyChanged(nameof(TotalSavingsSpent));
            OnPropertyChanged(nameof(TotalExpenses));

            OnPropertyChanged(nameof(NeedsBudget));
            OnPropertyChanged(nameof(WantsBudget));
            OnPropertyChanged(nameof(SavingsBudget));

            OnPropertyChanged(nameof(NeedsProgress));
            OnPropertyChanged(nameof(WantsProgress));
            OnPropertyChanged(nameof(SavingsProgress));

            OnPropertyChanged(nameof(NeedsSpentText));
            OnPropertyChanged(nameof(WantsSpentText));
            OnPropertyChanged(nameof(SavingsSpentText));

            OnPropertyChanged(nameof(NeedsRemainingText));
            OnPropertyChanged(nameof(WantsRemainingText));
            OnPropertyChanged(nameof(SavingsRemainingText));

            OnPropertyChanged(nameof(RemainingThisMonthText));

            OnPropertyChanged(nameof(DaysRemainingInMonth));
            OnPropertyChanged(nameof(DailyAverageSpending));
            OnPropertyChanged(nameof(TopSpendingCategory));
            OnPropertyChanged(nameof(BudgetStatus));
            OnPropertyChanged(nameof(BudgetStatusColor));
        }

        public void RefreshPage()
        {
            Recompute();
        }

        // --- INotifyPropertyChanged boilerplate ---
        public event PropertyChangedEventHandler? PropertyChanged;

        protected bool Set<T>(ref T field, T value, [CallerMemberName] string? name = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(name);
            return true;
        }

        public void OnPropertyChanged([CallerMemberName] string? name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
