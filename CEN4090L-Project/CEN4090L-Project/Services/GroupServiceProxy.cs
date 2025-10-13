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

       
    }
}
