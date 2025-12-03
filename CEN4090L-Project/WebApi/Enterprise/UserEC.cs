using CEN4090L_Project.Models;

namespace WebApi.Enterprise
{
    public class UserEC
    {
        public Group GetUsers()
        {
            return FakeDatabase.CurrentGroup;
        }

        public User? GetUserById(int id)
        {
            return FakeDatabase.CurrentGroup.UserList
                .FirstOrDefault(t => t.Id == id);
        }

        public User? Delete(int id)
        {
            var userToDelete = GetUserById(id);
            if (userToDelete != null)
            {
                FakeDatabase.CurrentGroup.UserList.Remove(userToDelete);
            }
            return userToDelete;
        }

        public User? AddOrUpdate(User? user)
        {
            if (user != null && user.Id == 0)
            {
                user.Id = FakeDatabase.NextKey;
                FakeDatabase.CurrentGroup.UserList.Add(user);
            }

            return user;
        }

        public User? ChangeCurrentUser(int id)
        {
            var user = GetUserById(id);
            if (user != null)
            {
                FakeDatabase.CurrentUser = user;
            }
            return user;
        }
    }
}
