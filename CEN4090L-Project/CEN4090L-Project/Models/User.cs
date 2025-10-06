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


        public override string ToString()
        {
            return $"{Id}-[{Username}] {Name} - {Email}";
        }
    }
}
