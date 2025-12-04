using CEN4090L_Project.Data;
using CEN4090L_Project.Models;
using Microsoft.EntityFrameworkCore;

namespace WebApi.Enterprise
{
    public class SavingGoalEC
    {
        private readonly AppDbContext _db;
        public SavingGoalEC(AppDbContext db)
        {
            _db = db;
        }
        public async Task<IEnumerable<SavingsGoal>> GetSavingsGoals()
        {
            return await _db.SavingsGoals.ToListAsync();
        }

        public async Task<SavingsGoal?> GetSavingsGoalById(int id)
        {
            return await _db.SavingsGoals.FirstOrDefaultAsync(sg => sg.Id == id);
        }

        public async Task Delete(int id)
        {
            var savingsgoal = await GetSavingsGoalById(id);
            if (savingsgoal != null)
            {
                _db.SavingsGoals.Remove(savingsgoal);
                await _db.SaveChangesAsync();
            }
        }
        public async Task<SavingsGoal> Add(SavingsGoal savingsGoal)
        {
            _db.SavingsGoals.Update(savingsGoal);
            await _db.SaveChangesAsync();
            return savingsGoal;
        }

        public async Task Update(int id, SavingsGoal savingsGoal)
        {
            if (id == savingsGoal.Id)
            {
                _db.SavingsGoals.Update(savingsGoal);
                await _db.SaveChangesAsync();
            }
        }
    }
}
