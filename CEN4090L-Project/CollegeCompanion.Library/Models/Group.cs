// Group.cs
using CEN4090L_Project.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace CEN4090L_Project.Models
{
    [Table("Group")]
    public class Group
    {
        // Properties

        public int Id { get; set; }

        public string? Name { get; set; }

        //// Private backing field for savings goal
        //private SavingsGoal? _savingsGoal;

        //public SavingsGoal? SavingsGoal
        //{
        //    get { return _savingsGoal; }
        //    set { _savingsGoal = value; }
        //}

        //// Budget
        //public decimal? Budget { get; set; }

        // Constructors
        public Group()
        {
            UserList = new List<User>();
        }

        public Group(int id, string name, SavingsGoal savingsGoal, decimal budget)
        {
            Id = id;
            Name = name;
            //SavingsGoal = savingsGoal;
            //Budget = budget;
        }

        public List<User>? UserList { get; set; }
    }
}
