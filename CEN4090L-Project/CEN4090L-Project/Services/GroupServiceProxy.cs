using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CEN4090L_Project.Models;

namespace CEN4090L_Project.Services
{
    public class GroupServiceProxy
    {
        private List<User> users;

        public List<User> Users 
        {
            get
            {
                return users;
            }
       
        }
        public GroupServiceProxy() { }

        public void AddorUpdateGroup(string groupName) { }

        public void DeleteGroup(string groupName) { }

       
    }
}
