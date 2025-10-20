using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BudgetApp;
using CEN4090L_Project.Models;

namespace CEN4090L_Project.Services
{
    public class GroupServiceProxy
    {
        private List<Group> groups;

        public List<Group> Groups
        {
            get
            {
                return groups;
            }
       
        }
        public GroupServiceProxy() { }

        private static object _lock = new object();
        private static GroupServiceProxy? instance;

        public static GroupServiceProxy Current 
        {
            get
            {
                lock (_lock)
                {
                    if (instance == null)
                    {
                        instance = new GroupServiceProxy();
                    }
                }

                return instance;
            }
        }

        public Group? AddorUpdateGroup(Group? group) { 
            if(group == null)
            {
                return group;
            }
            var newGroup = group.ID == 0;
      
            return group;
        }

        public void DeleteGroup(Group? group) {

           if(group == null)
            {
                return;
            }
           
           groups.Remove(group);
           
        }

        //public User? AddOrUpdateUser(User? user) { }

        public void DeleteUser(User? user) { }

        public void SwapGroup(Group? group) { }

        public void SwapUser(User? user) { }

       
    }
}
