using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CEN4090L_Project.Services;
using CEN4090L_Project.Models;

namespace CEN4090L_Project.Services
{
    public class TransactionServiceProxy
    {
        // should be get a singleton inst of group prox and run on that
        private static GroupServiceProxy? groupService;
        private static SavingsGoalService _savingsService = new SavingsGoalService();
        // get the user from group service
        public User? CurrentUser => GroupServiceProxy.Current.CurrentUser;

        // Singleton instance so we have a single proxy throughout the app
        private static TransactionServiceProxy? instance;
        public static TransactionServiceProxy Current
        {
            get
            {
                if (instance == null)
                {
                    instance = new TransactionServiceProxy();
                }
                return instance;
            }
        }
        // Private constructor
        private TransactionServiceProxy()
        {
            groupService = GroupServiceProxy.Current;
            // testing data
            AddOrUpdateBudget(new Budget {TotalAmount = 1000, Expenses = new List<Expense>()});
            CurrentUser?.Budget?.Expenses.Add(new Expense { Amount = 100, Title = "hello", Category = BudgetCategory.Needs});
        }

        public Budget AddOrUpdateBudget(Budget newBud)
        {
            CurrentUser.Budget = newBud;
            return CurrentUser.Budget;
        }

        //add desire expense from the list of expenses
        public void addExpense(Expense e)
        {
            CurrentUser?.Budget?.Expenses.Add(e);
        }

        //remove desire expense from the list of expenses
        public bool removeExpense(int id)
        {
            bool removed = false;
            foreach (Expense e in CurrentUser?.Budget?.Expenses ?? new List<Expense>())
            {
                //checks if the expense exiist in the list
                if (id == e.Id)
                {
                    removed = CurrentUser?.Budget?.Expenses.Remove(e) ?? false;

                }
            }
            return removed;
        }

        //METHODS
        //helper returns a list with the specified category
        //List<Expense> expenses takes either the budget expenses from User or Group expenses
        private static List<Expense>? returnCategory(List<Expense> expenses, BudgetCategory category)
        {
            if (category == BudgetCategory.Needs || category == BudgetCategory.Savings || category == BudgetCategory.Wants)
                return null;
            List<Expense> categoryList = new List<Expense>();

            foreach (Expense e in expenses)
            {
                if (e.Category == category)
                    categoryList.Add(e);
            }
            return categoryList;
        }

        // SavingsGoal wrappers
        // These forward calls to the SavingsGoalService using the current user context.

        public SavingsGoal AddOrUpdateSavingsGoal(SavingsGoal goal)
        {
            var user = CurrentUser ?? throw new InvalidOperationException("No current user available.");
            // ensure owner is set
            goal.UserId = user.Id;
            var saved = _savingsService.AddOrUpdate(goal);

            // Optionally re-run allocations when goals change:
            // if user.Budget?.TotalAmount is the monthly savings, call ReallocateMonthly(...)
            if (user.Budget?.Savings.HasValue == true && user.Budget.Savings.Value > 0)
            {
                // If your Budget stores a monthly savings number, pass it here.
                _savingsService.ReallocateMonthly(user.Budget.Savings.Value);
            }

            return saved;
        }

        public SavingsGoal? GetSavingsGoal(int id)
        {
            return _savingsService.Get(id);
        }

        public IReadOnlyList<SavingsGoal> ListAllSavingsGoals() => _savingsService.ListAll();

        public IReadOnlyList<SavingsGoal> ListActiveSavingsGoals() => _savingsService.ListActive();

        public bool DeleteSavingsGoal(int id)
        {
            return _savingsService.Delete(id);
        }

        public void ReallocateMonthlySavings(decimal monthlySavings)
        {
            _savingsService.ReallocateMonthly(monthlySavings);
        }
    }
}
