using System;

public class Budget
{
	//list of expense managed by the budget
    private List<Expense> expenses = new List<Expense>();

    public Budget() { }

	public string? Title { get; set; }

	public int? Priority { get; set; }

	public int? Expeneses { get; set; }

	//calculates the total expense per expenses
	public int? totalExpense { }

	//returns a list with the specified category
	public list<Expense> returnCategory (int id)
	{
	    //goes through the ID and returns list	
	}

	//remove desire expense from the list of expenses
	public void removeExpense(Expense e) { }

    //add desire expense from the list of expenses
    public void addExpense(Expense e) { }


}
