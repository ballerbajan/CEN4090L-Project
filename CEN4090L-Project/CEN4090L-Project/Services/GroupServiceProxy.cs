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
        private List<Group> groups = new List<Group>();

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

        private User? currentUser;

        public User? CurrentUser
        {
            get
            {
                // if we don't have a current user, set it to the first user in the current group
                // this implies that on user creation it must be assigned to a group
                if (currentUser == null)
                {
                    currentUser = currentGroup?.UserList?.FirstOrDefault();
                    return currentUser;
                }
                else
                {
                    return currentUser;
                }
            }
            set
            {
                currentUser = value;
            }
        }

        private Group? currentGroup;

        public Group? CurrentGroup
        {
            get
            {
                return currentGroup;
            }
            set
            {
                currentGroup = value;
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

        public User? AddOrUpdateUser(User? user)
        {
            if (user == null)
            {
                return user;
            }
            // Id 0 means new user
            if (user.Id == 0)
            {
                currentGroup?.UserList?.Add(user);
                return user;
            }
            else
            {
                // if user exists, update it
                var exitingUser = currentGroup?.UserList?.FirstOrDefault(u => u.Id == user.Id);
                if (exitingUser != null)
                {
                    currentGroup?.UserList?.Remove(exitingUser);
                    currentGroup?.UserList?.Add(user);
                    return user;
                }
                else
                {
                    currentGroup?.UserList?.Add(user);
                    return user;
                }
            }

        }

        public void DeleteUser(User? user) { }

        public void SwapGroup(Group? group)
        {
            CurrentGroup = group;
        }

        public void SwapUser(User? user)
        {
            CurrentUser = user;
        }

       
    }
}
