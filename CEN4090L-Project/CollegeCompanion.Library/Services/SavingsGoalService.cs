using System;
using System.Collections.Generic;
using System.Linq;
using CEN4090L_Project.Models;

namespace CEN4090L_Project.Services
{
    /// <summary>
    /// In-memory service for CRUD + allocation.
    /// </summary>
    public class SavingsGoalService
    {
        private readonly List<SavingsGoal> _goals = new();

        // ---------- CRUD ----------
        public SavingsGoal AddOrUpdate(SavingsGoal goal)
        {
            // basic guards
            if (string.IsNullOrWhiteSpace(goal.Name))
                throw new ArgumentException("Goal name is required.", nameof(goal));
            if (goal.TargetAmount <= 0)
                throw new ArgumentException("TargetAmount must be > 0.", nameof(goal));

            // cap current to target
            if (goal.CurrentAmount > goal.TargetAmount)
                goal.CurrentAmount = goal.TargetAmount;

            if (goal.Id == 0)
            {
                goal.Id = _goals.Count == 0 ? 1 : _goals.Max(g => g.Id) + 1;
                _goals.Add(goal);
            }
            else
            {
                var i = _goals.FindIndex(g => g.Id == goal.Id);
                if (i >= 0) _goals[i] = goal;
                else _goals.Add(goal);
            }
            return goal;
        }

        public SavingsGoal? Get(int id) => _goals.FirstOrDefault(g => g.Id == id);

        public IReadOnlyList<SavingsGoal> ListAll() => _goals.AsReadOnly();

        public IReadOnlyList<SavingsGoal> ListActive() =>
            _goals.Where(g => g.IsActive && !g.IsAchieved).ToList();

        public bool Delete(int id)
        {
            var g = _goals.FirstOrDefault(x => x.Id == id);
            if (g is null) return false;
            g.IsActive = false;            // soft delete to preserve history
            g.MonthlyAllocation = 0m;
            return true;
        }

        // ---------- Allocation ----------
        /// <summary>
        /// Splits the user's monthly savings budget across active, unfinished goals.
        /// Equal split by default, with leftover cents pushed forward in list order.
        /// Re-run this any time the list of goals or the monthly budget changes.
        /// </summary>
        public void ReallocateMonthly(decimal monthlySavings)
        {
            if (monthlySavings < 0) monthlySavings = 0;

            var goals = _goals
                .Where(g => g.IsActive && !g.IsAchieved)
                .OrderByDescending(g => g.Priority) // higher priority earlier for leftovers
                .ToList();

            foreach (var g in goals) g.MonthlyAllocation = 0m;
            if (goals.Count == 0 || monthlySavings == 0) return;

            // equal base share (to cents)
            var baseShare = Math.Floor((monthlySavings / goals.Count) * 100m) / 100m;

            decimal assigned = 0m;
            foreach (var g in goals)
            {
                var alloc = Math.Min(baseShare, g.RemainingAmount);
                g.MonthlyAllocation = alloc;
                assigned += alloc;
            }

            // distribute leftover cents or unused share to goals with remaining room, by priority order
            var leftover = monthlySavings - assigned;
            if (leftover <= 0) return;

            foreach (var g in goals)
            {
                var room = g.RemainingAmount - g.MonthlyAllocation;
                if (room <= 0) continue;

                var add = Math.Min(leftover, room);
                // round to cents as we allocate leftovers
                add = Math.Floor(add * 100m) / 100m;

                g.MonthlyAllocation += add;
                leftover -= add;

                if (leftover <= 0) break;
            }
        }
    }
}


/*
--------------------------------------------------------------

CLASS: SavingsGoalService
- Handles CRUD (Create, Read, Update, Delete) operations for savings goals.
- Maintains an in-memory list of all goals. This can later be replaced with a database connection.
- Core Methods:
    • AddOrUpdate(SavingsGoal goal)
        → Adds a new goal or updates an existing one.
        → Validates required fields like Name and TargetAmount.
    • Get(int id)
        → Retrieves a specific goal by its ID.
    • ListAll()
        → Returns all goals, including inactive or completed ones.
    • ListActive()
        → Returns only active, unfinished goals.
    • Delete(int id)
        → Marks a goal as inactive (soft delete) and clears its monthly allocation.
    • ReallocateMonthly(decimal monthlySavings)
        → Automatically splits the user’s monthly savings amount across all active goals.
        → Each goal receives an equal base share, with leftover amounts distributed
          according to priority and remaining balance.
        → Called whenever a new goal is added, removed, or completed.

--------------------------------------------------------------
*/
