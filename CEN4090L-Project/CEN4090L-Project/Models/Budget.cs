using System;

public class Budget
{
    //list of all expense managed by the budget
    private List<Expense> expenses = new List<Expense>();

    //list for every expense category
    private List<Expense> needs = new List<Expense>();
    private List<Expense> wants = new List<Expense>();
    private List<Expense> savings = new List<Expense>();

    private decimal? totalExpenses;

    public Budget() { }

    public List<Expense> Expeneses {
        get{ return expenses;}
    }

    //calculates the total expense per expenses
    public decimal? totalExpense
    {
        get
        {
            decimal? totalExpenses = 0;
            foreach (Expense e in Expenses)
            {
                totalExpenses += e.amount;
            }
            return totalExpenses;
        }
	}

    public decimal? Needs{ get; set;}

    public decimal? Wants{ get; set;}

    public decimal? Savings{ get; set;}

    
    

   


}