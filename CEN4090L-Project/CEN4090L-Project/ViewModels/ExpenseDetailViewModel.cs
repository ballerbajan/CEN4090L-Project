using System;
using CEN4090L_Project.Models;
using CEN4090L_Project.Services;
namespace CEN4090L_Project.ViewModels
{
    public class ExpensePageViewModel
    {
        private Expense Model { get; set; }
        public ExpensePageViewModel()
        {
            Model = new Expense();
        }

        public void AddOrUpdateExpense()
        {
            TransactionServiceProxy.Current.addExpense(Model);
        }
    }
}