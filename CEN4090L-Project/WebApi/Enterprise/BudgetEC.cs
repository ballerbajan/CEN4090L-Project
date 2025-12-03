using CEN4090L_Project.Data;
using Microsoft.EntityFrameworkCore;

namespace WebApi.Enterprise
{
    // controller intermediary for budget operations
    public class BudgetEC
    {
        private readonly AppDbContext _db;

        public BudgetEC(AppDbContext db)
        {
            _db = db;
        }

        public async Task<Budget?> GetBudgetById(int id)
        {
            return await _db.Budgets.FirstOrDefaultAsync(b => b.Id == id);
        }

        public async Task<Budget> Add(Budget budget)
        {
            _db.Budgets.Update(budget);
            await _db.SaveChangesAsync();
            return budget;
        }

        public async Task Update(int id, Budget budget)
        {
           if (id == budget.Id)
           {
                _db.Budgets.Update(budget);
                await _db.SaveChangesAsync();
           }
        }

        public async Task Delete(int id)
        {
            var budget = await GetBudgetById(id);
            if (budget != null)
            {
                _db.Budgets.Remove(budget);
                await _db.SaveChangesAsync();
            }
        }
    }
}

