using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CEN4090L_Project.Models
{
    [Table("Users")]
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("Id")]
        public int Id { get; set; }

        [Column("Name")]
        public string? Name { get; set; }

        [Column("Email")]
        public string? Email { get; set; }

        [Column("Password")]
        public string? Password { get; set; }

        [Column("Username")]
        public string? Username { get; set; }

        [Column("Budget_Id")]
        public Budget? Budget { get; set; }

        [Column("Income")]
        public float? Income { get; set; } // tracked monthly income just a float

        [Column("SavingsGoals_id")]
        public List<SavingsGoal> SavingsGoals { get; set; } = new List<SavingsGoal>();

        public override string ToString()
        {
            return $"{Id}-[{Username}] {Name} - {Email}";
        }
    }
}
