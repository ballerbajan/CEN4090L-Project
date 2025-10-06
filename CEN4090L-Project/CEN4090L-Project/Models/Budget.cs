using System;
using System.Collections.Generic;

public class Budget
{
	//list of expense managed by the budget
    private List<Expenses> expenses = new List<Expenses>();

    public Budget() { }

	public string? Title { get; set; }

	public int? Priority { get; set; }

	public int? Expeneses { get; set; }

	//calculates the total expense per expenses
	public int? totalExpense {
		get; set;
	}

	//returns a list with the specified category
	public List<Expenses>? returnCategory (int id)
	{
	    //goes through the ID and returns list	
		throw new NotImplementedException();
    }

	//remove desire expense from the list of expenses
	public void removeExpense(Expenses e) { }

    //add desire expense from the list of expenses
    public void addExpense(Expenses e) { }


}
