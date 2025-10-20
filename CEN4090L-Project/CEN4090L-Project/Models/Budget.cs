using CEN4090L_Project.Models;
using System;

public class Budget
{
    //list of all expense managed by the budget
    private List<Expense> expenses = new List<Expense>();

    //list for every expense category
    private List<Expense> needsList = new List<Expense>();
    private List<Expense> wantsList = new List<Expense>();
    private List<Expense> savingsList = new List<Expense>();

    private decimal? totalAmount;

    public Budget(need, want, saving)
    {
        Needs = need;
        Wants = want;
        Savings = saving;
    }

    public List<Expense> Expenses {
        get{ return expenses;}
    }

    private decimal? needs;
  
    private decimal? wants;
  
    private decimal? savings;
    
    public decimal? Needs{ get; set;}

    public decimal? Wants{ get; set;}

    public decimal? Savings{ get; set;}

    //calculates the total expense per expenses
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
}