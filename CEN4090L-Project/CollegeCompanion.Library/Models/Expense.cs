using CEN4090L_Project.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Drawing;

[Table("Expense")]
public class Expense
{
    public DateTime Date;

    public Expense() {}

    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("Id")]
    public int Id { get; set; }

    [Column("Title")]
    public string? Title { get; set; }

    [Column("Amount", TypeName = "decimal")]
    public decimal? Amount { get; set; }

    [Column("Description")]
    public string? Description { get; set; }

    [Column("Category")]
    public BudgetCategory? Category { get; set; }

    [Column("DateTime", TypeName = "timestamp")]
    public DateTime DateTime { get; set; } = DateTime.Now;
    public Color CategoryColor => Category switch
    {
        BudgetCategory.Needs => ColorTranslator.FromHtml("#3B82F6"),
        BudgetCategory.Wants => ColorTranslator.FromHtml("#F59E0B"),
        BudgetCategory.Savings => ColorTranslator.FromHtml("#10B981"),
        _ => ColorTranslator.FromHtml("#6B7280")
    };

    [NotMapped]
    public string CategoryName => Category?.ToString() ?? "Uncategorized";

    public override string ToString()
	{
		return $"{Title}: {Amount}";
    }
}
