using System;
using CEN4090L_Project.Models;

public class Expense
{
	public Expense() {}

	public string? Title { get; set; }

	public decimal? Amount { get; set; }

    public BudgetCategory? Category { get; set; }

	public override string ToString()
	{
		return $"{Title}: {Amount}";
    }
}
