// Group.cs
using System.Collections.Generic;

namespace BudgetApp
{
    public class Group
    {
        // Properties
        public int ID { get; set; }
        public string Name { get; set; }

        // Private backing field for list of users
        private List<string> _users = new List<string>();
        public List<string> Users
        {
            get { return _users; }
            set { _users = value ?? new List<string>(); }
        }

        // List of savings goals
        public List<string> SavingsGoals { get; set; } = new List<string>();

        // Budget
        public decimal Budget { get; set; }

        // Constructor
        public Group(int id, string name, decimal budget)
        {
            ID = id;
            Name = name;
            Budget = budget;
        }

        // Default constructor
        public Group() { }

        // Example methods
        public void AddUser(string user)
        {
            if (!string.IsNullOrWhiteSpace(user) && !_users.Contains(user))
                _users.Add(user);
        }

        public void AddSavingsGoal(string goal)
        {
            if (!string.IsNullOrWhiteSpace(goal))
                SavingsGoals.Add(goal);
        }
    }
}
