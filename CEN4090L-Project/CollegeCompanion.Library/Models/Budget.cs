using CEN4090L_Project.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

[Table("Budget")]
public class Budget
{
    // List of all expense managed by the budget
    private List<Expense> expenses = new List<Expense>();

    //// List for every expense category
    //private List<Expense> needsList = new List<Expense>();
    //private List<Expense> wantsList = new List<Expense>();
    //private List<Expense> savingsList = new List<Expense>();

    private decimal? totalAmount;
    private decimal? needs;
    private decimal? wants;
    private decimal? savings;

    public Budget() { }

    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("Id")]
    public int Id { get; set; }
    public int UserId { get; set; }
    public User? User { get; set; } // navigation property
    public Budget(decimal need, decimal want, decimal saving)
    {
        Needs = need;
        Wants = want;
        Savings = saving;
    }

    public List<Expense> Expenses
    {
        get { return expenses; }
        set { expenses = value; }
    }

    [Column("Needs")]
    public decimal? Needs { get; set; }

    [Column("Wants")]
    public decimal? Wants { get; set; }

    [Column("Savings")]
    public decimal? Savings { get; set; }

    // ADD THESE NEW PROPERTIES for UI display:

    // Spent amounts (calculated from expenses)
    public decimal NeedsSpent =>
        expenses?.Where(e => e.Category == BudgetCategory.Needs).Sum(e => e.Amount ?? 0) ?? 0;

    public decimal WantsSpent =>
        expenses?.Where(e => e.Category == BudgetCategory.Wants).Sum(e => e.Amount ?? 0) ?? 0;

    public decimal SavingsSpent =>
        expenses?.Where(e => e.Category == BudgetCategory.Savings).Sum(e => e.Amount ?? 0) ?? 0;

    // Remaining amounts
    public decimal NeedsRemaining => (Needs ?? 0) - NeedsSpent;
    public decimal WantsRemaining => (Wants ?? 0) - WantsSpent;
    public decimal SavingsRemaining => (Savings ?? 0) - SavingsSpent;

    // Allocated amounts (for display)
    public decimal NeedsAllocated => Needs ?? 0;
    public decimal WantsAllocated => Wants ?? 0;
    public decimal SavingsAllocated => Savings ?? 0;

    [Column("TotalAmount")]
    // Calculates the total budget while setting the expenses. Gets the total amount
    public decimal? TotalAmount
    {
        get
        {
            return totalAmount;
        }
        set // on set we also want to recalculate the needs, wants, and savings
        {
            // we need to check each expense category and remove it from needs, wants, or savings
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