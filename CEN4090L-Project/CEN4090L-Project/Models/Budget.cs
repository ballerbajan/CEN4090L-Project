using System;

public class Budget
{
    //list of expense managed by the budget
    private List<Expense> expenses = new List<Expense>();

    public Budget() { }

    public string? Title { get; set; }

    public int? Priority { get; set; }

    public List<Expense> Expenses
    {
        get
        {
            return expenses;
        }
    }

    //calculates the total expense per expenses
    public decimal? totalExpense()
    {
        decimal? totalExpenses = 0;
	    foreach (Expense e in Expenses)
        {
           totalExpenses += e.Amount;
        }
		return totalExpenses;
	}

//returns a list with the specified category
public List<Expense>? returnCategory(int id)
{
    if (id < 0 && id > 3)
        return null;
    List<Expense> category = new List<Expense>();

    foreach (Expense e in Expenses)
    {
      if (e.Id == id)
        category.Add(e);
    }
    return category;
}

//remove desire expense from the list of expenses
public bool removeExpense(Expense e1)
{
    bool removed = false;
    foreach (Expense e in Expenses)
    {
        //checks if the expense exiist in the list
        if (e.Title == e1.Title)
        {
            removed = Expenses.Remove(e1);

        }
    }
    return removed;
}

//add desire expense from the list of expenses
public void addExpense(Expense e)
{
    Expenses.Add(e);
}


}