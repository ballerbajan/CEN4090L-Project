// Group.cs
using CEN4090L_Project.Models;

namespace CEN4090L_Project.Models
{
    public class Group
    {
        // Properties
        public int ID { get; set; }
        public string? Name { get; set; }

        // Private backing field for savings goal
        private SavingsGoal? _savingsGoal;
        public SavingsGoal? SavingsGoal
        {
            get { return _savingsGoal; }
            set { _savingsGoal = value; }
        }

        // Budget
        public decimal? Budget { get; set; }

        // Constructors
        public Group()
        {
            UserList = new List<User>();
        }

        public Group(int id, string name, SavingsGoal savingsGoal, decimal budget)
        {
            ID = id;
            Name = name;
            SavingsGoal = savingsGoal;
            Budget = budget;
        }

        public List<User>? UserList { get; set; }
    }
}
