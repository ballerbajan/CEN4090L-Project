using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace CEN4090L_Project.ViewModels
{
    // --- simple models used by the dashboard ---
    public class Expense
    {
        public string Description { get; set; } = string.Empty;
        public string Category { get; set; } = "Needs"; // Needs/Wants/Savings
        public decimal Amount { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;
    }

    public class DashboardViewModel : INotifyPropertyChanged
    {
        // ----- backing fields -----
        private decimal _income = 0m;           // no default value - user must enter
        private decimal _plannedSavings = 0m;   // no default value - user must enter
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
            set { Set(ref _recent, value); Recompute(); }
        }

        // computed totals
        public decimal TotalNeeds => RecentExpenses.Where(e => e.Category == "Needs").Sum(e => e.Amount);
        public decimal TotalWants => RecentExpenses.Where(e => e.Category == "Wants").Sum(e => e.Amount);
        public decimal TotalSavingsSpent => RecentExpenses.Where(e => e.Category == "Savings").Sum(e => e.Amount);
        public decimal TotalExpenses => TotalNeeds + TotalWants + TotalSavingsSpent;

        // allocation targets
        public decimal NeedsBudget => Math.Round(Income * 0.50m, 2);
        public decimal WantsBudget => Math.Round(Income * 0.30m, 2);
        public decimal SavingsBudget => Math.Round(Income * 0.20m, 2);

        // progress 0..1
        public double NeedsProgress => (double)Math.Clamp(NeedsBudget == 0 ? 0 : TotalNeeds / NeedsBudget, 0, 1);
        public double WantsProgress => (double)Math.Clamp(WantsBudget == 0 ? 0 : TotalWants / WantsBudget, 0, 1);
        public double SavingsProgress => (double)Math.Clamp(SavingsBudget == 0 ? 0 : TotalSavingsSpent / SavingsBudget, 0, 1);

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

        public DashboardViewModel()
        {
            // Initialize with empty collection - user will add their own expenses
            RecentExpenses = new ObservableCollection<Expense>();

            EditBudgetCommand = new Command(OnEditBudget);
            AddExpenseCommand = new Command(OnAddExpense);
        }

        private void OnEditBudget()
        {
            // TODO: navigate to Budget Setup/Edit Page (Issue #21)
            Application.Current?.MainPage?.DisplayAlert("Edit Budget", "Navigate to Budget Setup/Edit Page.", "OK");
        }

        private void OnAddExpense()
        {
            // TODO: navigate to Add Expense Form (Issue #19)
            Application.Current?.MainPage?.DisplayAlert("Add Expense", "Navigate to Add Expense Form.", "OK");
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
        protected void OnPropertyChanged([CallerMemberName] string? name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
