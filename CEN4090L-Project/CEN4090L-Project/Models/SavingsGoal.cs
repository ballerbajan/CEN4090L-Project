using System;
using System.Text.Json.Serialization;

namespace CEN4090L_Project.Models
{
    public class SavingsGoal
    {
        public int Id { get; set; }                         // 0 = new (not saved yet)
        public int OwnerUserId { get; set; }

        public string Name { get; set; } = string.Empty;
        public string? Category { get; set; }

        public decimal TargetAmount { get; set; }           // e.g., 800.00
        public decimal CurrentAmount { get; set; } = 0m;    // how much saved so far

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? TargetDate { get; set; }           // optional deadline

        public int Priority { get; set; } = 3;              // 1..5
        public bool IsActive { get; set; } = true;          // soft-delete / archive

        // how much of this month’s savings budget is allocated to this goal
        public decimal MonthlyAllocation { get; set; } = 0m;

        // -------- computed / UI helpers --------
        [JsonIgnore]
        public decimal RemainingAmount => Math.Max(0m, TargetAmount - CurrentAmount);

        [JsonIgnore]
        public decimal PercentComplete =>
            TargetAmount <= 0 ? 0 : Math.Clamp(CurrentAmount / TargetAmount, 0, 1);

        [JsonIgnore]
        public bool IsAchieved => CurrentAmount >= TargetAmount;

        public override string ToString() =>
            $"{Name} — {CurrentAmount:C}/{TargetAmount:C} ({PercentComplete:P0})";
    }
}

/*
--------------------------------------------------------------
Explanation:
This file implements the SavingsGoal feature for the College Companion app.

CLASS: SavingsGoal
- Represents an individual savings target created by a student user.
- Stores details such as:
    • Id – Unique identifier for the goal.
    • OwnerUserId – ID of the user who owns the goal.
    • Name – The name of the savings goal (e.g., “Spring Break Trip”).
    • Category – Optional category label for organization.
    • TargetAmount – The total amount the user aims to save.
    • CurrentAmount – How much has been saved so far.
    • Priority – A ranking (1–5) to help distribute monthly savings.
    • MonthlyAllocation – How much of the monthly budget is assigned to this goal.
    • IsActive – Indicates whether the goal is currently active.
    • CreatedAt / TargetDate – Track creation time and optional deadline.
- Computed Properties:
    • RemainingAmount – Calculates how much is left to reach the goal.
    • PercentComplete – Returns progress as a percentage.
    • IsAchieved – True if the goal’s target has been reached.
- ToString() – Formats the goal’s progress neatly for display in the UI.

--------------------------------------------------------------
*/

