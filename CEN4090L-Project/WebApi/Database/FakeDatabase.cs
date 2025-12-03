using CEN4090L_Project.Models;

namespace WebApi.Database
{
    public static class FakeDatabase
    {
        public static int NextKey
        {
            get
            {
                if (ToDos.Any())
                {
                    return ToDos.Select(t => t.Id).Max() + 1;
                }
                return 1;
            }
        }

        private static List<Group> groups = new List<Group>()
        {
            new Group()
            {
                ID = 1,
                Name = "Test Group",
                Budget = 1000,
                UserList = new List<User>()
                {
                }
            }
        };

        public static List<Group> Groups => groups;
        public static Group CurrentGroup => groups[0];
        private static User? currentUser;
        public static User CurrentUser
        {
            get
            {
                if (currentUser == null)
                {
                    currentUser = CurrentGroup.UserList.FirstOrDefault() ?? new User();
                }
                return currentUser;
            }
        }


    }
}
