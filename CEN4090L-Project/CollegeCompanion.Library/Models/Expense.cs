using System;
using System.Drawing;
using CEN4090L_Project.Models;

public class Expense
{
    public DateTime Date;

    public Expense() {}
	public int Id { get; set; }
	public string? Title { get; set; }
	public decimal? Amount { get; set; }

    public string? Description { get; set; }
    public BudgetCategory? Category { get; set; }
    public DateTime DateTime { get; set; } = DateTime.Now;
    public Color CategoryColor => Category switch
    {
        BudgetCategory.Needs => ColorTranslator.FromHtml("#3B82F6"),
        BudgetCategory.Wants => ColorTranslator.FromHtml("#F59E0B"),
        BudgetCategory.Savings => ColorTranslator.FromHtml("#10B981"),
        _ => ColorTranslator.FromHtml("#6B7280")
    };


    public string CategoryName => Category?.ToString() ?? "Uncategorized";



    public override string ToString()
	{
		return $"{Title}: {Amount}";
    }
}
