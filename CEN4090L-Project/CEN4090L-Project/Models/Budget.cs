using System;

public class Budget
{
    //list of expense managed by the budget
    private List<Expense> expenses = new List<Expense>();

    public Budget() { }

    public string? Title { get; set; }

    public int? Priority { get; set; }

    public list<Expense> Expeneses { get; set; }

    //calculates the total expense per expenses
    public decimal? totalExpense
    {

        decimal? totalExpenses = 0;
	    foreach (e in Expenses)
		   totalExpenses += e.amount;
		return totalExpenses;
	}

//returns a list with the specified category
public list<Expense> returnCategory(int id)
{
    if (id < 0 && id > 3)
        return null;
    list<Expense> category = new List<Expense>();

    foreach (e in Expenses)
        if (e.id == id)
            category.add(e);

    return category;


}

//remove desire expense from the list of expenses
public void removeExpense(Expense e1)
{
    bool removed = 0;
    foreach (e in Expenses)
    {
        //checks if the expense exiist in the list
        if (e.title == e1.title)
        {
            removed = Expenses.remove(e1);

        }
    }
    return removed;
}

//add desire expense from the list of expenses
public void addExpense(Expense e)
{
    Expenses.add(e);
}


}