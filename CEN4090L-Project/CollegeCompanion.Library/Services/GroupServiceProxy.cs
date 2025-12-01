using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BCrypt.Net;
using CEN4090L_Project.Models;
using CollegeCompanion.Library.Utilities;
using Newtonsoft.Json;

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
        public GroupServiceProxy() {
            var groupData = new WebRequestHandler().Get("/Group/Expand").Result;
            groups = JsonConvert.DeserializeObject<List<Group>>(groupData) ?? new List<Group>();
        
        
        }

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
                // if we don't have any groups, create one and set it as current
                if (groups.FirstOrDefault() == null)
                {
                    groups.Add(new Group());
                    currentGroup = groups.FirstOrDefault();
                }
                // TODO: if we are still planning to have multiple groups we should NOT
                // automatically set the current group to the first one here

                // if we don't have a current group, set it to the first group
                if (currentGroup == null)
                {
                    currentGroup = groups.FirstOrDefault();
                }
                return currentGroup;
            }
            set
            {
                currentGroup = value;
            }
        }

        private int NextUserId
        {
            // everytime this int is fetched the code inside of it runs
            get
            {
                if (currentGroup != null && (currentGroup?.UserList?.Any() ?? false))
                {
                    return currentGroup.UserList.Select(u => u.Id).Max() + 1;
                }
                return 1;
            }
        }
        
        public Group? AddorUpdateGroup(Group? group)
        {
            if (group == null)
            {
                return group;
            }
            if (group.ID == 0)
            {
                groups.Add(group);
                return group;
            }
            else
            {
                var existingGroup = groups.FirstOrDefault(g => g.ID == group.ID);
                if (existingGroup != null)
                {
                    var Index = groups.IndexOf(existingGroup);
                    groups.RemoveAt(Index);
                    groups.Insert(Index, existingGroup);
                    return group;
                }
                groups.Add(group);
                return group;
                
            }
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
            var isNewUser = user.Id == 0;
            var userData = new WebRequestHandler().Post("/Users", user).Result;
            var newProject = JsonConvert.DeserializeObject<User>(userData);
            // Id 0 means new user
            if (user.Id == 0)
            {
                user.Id = NextUserId;
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

        public void DeleteUser(User? user) {
            if (user == null) {
                return;
            }

            currentGroup?.UserList?.Remove(user);

        }

        public void SwapGroup(Group? group)
        {
            if (group?.ID != null)
            {
                CurrentGroup = group;
            }

            return;
           
        }

        public void SwapUser(User? user)
        {
            var existinguser = currentGroup?.UserList?.FirstOrDefault(u => u.Id == user?.Id);
            if(existinguser != null) 
            {
                CurrentUser = user;
            }
            return;
        }

        // Registers a new user in the current group
        public bool Register(string username, string pass, string name, string email)
        {

            // check if username already exists in current group if we get null also return false
            // since there would be no group or user to add
            // if so return false
            if (CurrentGroup?.UserList?.Any(u => u.Username == username) ?? false)
            {
                return false; // registration failed
            }
            User user = new User
            {
                Id = NextUserId,
                Username = username,
                Password = BCrypt.Net.BCrypt.HashPassword(pass),
                Name = name,
                Email = email
            };

            CurrentGroup?.UserList?.Add(user);

            return true; // registration successful
        }
       
        public bool Login(string username, string pass)
        {
            // TODO: if we are still planning to have multiple groups we
            // need to check each group for the username

            // check if username exists in current group
            var user = CurrentGroup?.UserList?.FirstOrDefault(u => u.Username == username);
            if (user != null && BCrypt.Net.BCrypt.Verify(pass, user.Password))
            {
                CurrentUser = user;
                return true; // login successful
            }
            return false; // login failed
        }
    }
}
