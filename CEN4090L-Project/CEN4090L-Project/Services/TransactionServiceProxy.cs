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
        private GroupServiceProxy groupService = GroupServiceProxy.Current;
        // should be this
        // var user = groupService.currentUser;
        // for now just get the first user
        User user = groupService.Users[0];

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
            // testing data
            user.Budget.Add(new Budget() { Title = "Food", Priority = 1 });
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
            Expenses.add(e);
        }

        //remove desire expense from the list of expenses
        public bool removeExpense(int id)
        {
            bool removed = 0;
            foreach (Expense e in Expenses)
            {
                //checks if the expense exiist in the list
                if (id == e.id)
                {
                    removed = Expenses.remove(e);

                }
            }
            return removed;
        }

        //METHODS
        //helper returns a list with the specified category
        //NOTE: the int ID is gonna be changed for BudgetCategory
        private static list<Expense>? returnCategory(BudgetCategory category)
        {
            if (category == BudgetCategory.Needs || category == BudgetCategory.Savings || category == BudgetCategory.Wants)
                return null;
            List<Expense> categoryList = new List<Expense>();

            foreach (Expense e in Expenses)
            {
                if (e.Category == category)
                    categoryList.Add(e);
            }
            return categoryList;
        }

    }
}
