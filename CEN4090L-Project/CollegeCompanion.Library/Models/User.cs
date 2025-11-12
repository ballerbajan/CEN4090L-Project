using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CEN4090L_Project.Models
{
    public class User
    {
        public int Id { get; set; }
        public string? Name { get; set; }

        public string? Email { get; set; }

        public string? Password { get; set; }

        public string? Username { get; set; }

        public Budget? Budget { get; set; }

        public float? Income { get; set; } // tracked monthly income just a float

        public List<SavingsGoal> SavingsGoals { get; set; } = new List<SavingsGoal>();

        public override string ToString()
        {
            return $"{Id}-[{Username}] {Name} - {Email}";
        }
    }
}
