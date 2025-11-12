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

        // should be this
        // var user = groupService.currentUser;
        // for now just get the first user
        private User user = groupService?.CurrentUser ?? new User();

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
            user?.Budget?.Expenses.Add(new Expense { Amount = 100, Title = "hello", Category = BudgetCategory.Needs});
        }

        // this should take in a budget object but for now just the fields
        public Budget AddOrUpdateBudget(Budget newBud)
        {
            user.Budget = newBud;
            return user.Budget;
        }

        //add desire expense from the list of expenses
        public void addExpense(Expense e)
        {
            user?.Budget?.Expenses.Add(e);
        }

        //remove desire expense from the list of expenses
        public bool removeExpense(int id)
        {
            bool removed = false;
            foreach (Expense e in user?.Budget?.Expenses ?? new List<Expense>())
            {
                //checks if the expense exiist in the list
                if (id == e.Id)
                {
                    removed = user?.Budget?.Expenses.Remove(e) ?? false;

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

    }
}
