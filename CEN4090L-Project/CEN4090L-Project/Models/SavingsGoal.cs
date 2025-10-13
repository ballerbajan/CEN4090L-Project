using System;
using System.Text.Json.Serialization;

namespace CEN4090L_Project.Models
{
    public class SavingsGoal
    {
        public int Id { get; set; }
        public int OwnerUserId { get; set; }

        public string Name { get; set; } = string.Empty;
        public decimal TargetAmount { get; set; }
        public decimal CurrentAmount { get; set; } = 0m;

        public DateTime StartDate { get; set; } = DateTime.UtcNow;
        public DateTime? TargetDate { get; set; }

        public string Status { get; set; } = "Active"; // e.g. Active, Achieved, Paused
        public int Priority { get; set; } = 3; // scale 1–5
        public string? Category { get; set; }

    }
}
