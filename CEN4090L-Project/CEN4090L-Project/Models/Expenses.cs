using System;

public class Expenses
{
    public Expenses() { }

    public string? Title { get; set; }

    public decimal? Amount { get; set; }

    public string? Category { get; set; }

    public override string ToString()
    {
        return $"{Title}: {Amount}";
    }
}