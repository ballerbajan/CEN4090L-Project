// Group.cs
namespace BudgetApp
{
    public class Group
    {
        // Properties
        public int ID { get; set; }
        public string Name { get; set; }

        // Private backing field for user
        private string _user;
        public string User
        {
            get { return _user; }
            set { _user = value; }
        }

        // Private backing field for savings goal
        private string _savingsGoal;
        public string SavingsGoal
        {
            get { return _savingsGoal; }
            set { _savingsGoal = value; }
        }

        // Budget
        public decimal Budget { get; set; }

        // Constructors
        public Group() { }

        public Group(int id, string name, string user, string savingsGoal, decimal budget)
        {
            ID = id;
            Name = name;
            User = user;
            SavingsGoal = savingsGoal;
            Budget = budget;
        }
    }
}
