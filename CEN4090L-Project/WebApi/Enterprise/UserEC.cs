using CEN4090L_Project.Data;
using CEN4090L_Project.Models;
using Microsoft.EntityFrameworkCore;

namespace WebApi.Enterprise
{
    public class UserEC
    {
        private readonly AppDbContext _db;
        public UserEC(AppDbContext db)
        {
            _db = db;
        }
        public async Task<IEnumerable<User>> GetUsers()
        {
            return await _db.Users.ToListAsync();
        }

        public async Task<User?> GetUserById(int id)
        {
            return await _db.Users.FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task Delete(int id)
        {
            var user = await GetUserById(id);
            if (user != null)
            {
                _db.Users.Remove(user);
                await _db.SaveChangesAsync();
            }
        }

        public async Task<User> Add(User user)
        {
            _db.Users.Update(user);
            await _db.SaveChangesAsync();
            return user;
        }

        public async Task Update(int id, User user)
        {
            if (id == user.Id)
            {
                _db.Users.Update(user);
                await _db.SaveChangesAsync();
            }
        }

        //public User? ChangeCurrentUser(int id)
        //{
        //    var user = GetUserById(id);
        //    if (user != null)
        //    {
        //        FakeDatabase.CurrentUser = user;
        //    }
        //    return user;
        //}
    }
}
