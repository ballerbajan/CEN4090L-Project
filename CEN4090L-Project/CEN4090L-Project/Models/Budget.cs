using CEN4090L_Project.Models;
using System;

public class Budget
{
    //list of expense managed by the budget
    private List<Expense> expenses = new List<Expense>();

    public List<Expense> Expenses
    {
        get 
        { 
            return expenses; 
        }
    }

    private decimal? totalAmount;
    private decimal? needs;
    private decimal? wants;
    private decimal? savings;

    public decimal? Needs 
    { 
        get 
        { 
            return needs; 
        }
    }

    public decimal? Wants 
    { 
        get 
        { 
            return wants; 
        }
    }   

    public decimal? Savings 
    { 
        get 
        { 
            return savings; 
        }
    }

    public decimal? TotalAmount
    {
        get 
        { 
            return totalAmount; 
        }
        set // on set we also want to recalculate the needs, wants, and savings
        {
            // we nee to check each expense catergory and remove it from needs, wants, or savings
            needs = value * 0.5m;

            // where returns an IEnumerable so then we run sum on it
            needs -= expenses.Where(e => e.Category == BudgetCategory.Needs).Sum(e => e.Amount) ?? 0;

            wants = value * 0.3m;
            wants -= expenses.Where(e => e.Category == BudgetCategory.Wants).Sum(e => e.Amount) ?? 0;

            savings = value * 0.2m;
            savings -= expenses.Where(e => e.Category == BudgetCategory.Savings).Sum(e => e.Amount) ?? 0;

            totalAmount = value; 
        }
    }

    public List<Expense> Expeneses {
        get
        {
            return expenses;
        }
    }
}