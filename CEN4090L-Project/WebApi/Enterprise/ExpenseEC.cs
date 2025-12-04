using CEN4090L_Project.Data;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace WebApi.Enterprise
{
    public class ExpenseEC
    {
        private readonly AppDbContext _db;
        public ExpenseEC(AppDbContext db)
        {
            _db = db;
        }
        public async Task<IEnumerable<Expense>> GetExpenses()
        {
            return await _db.Expenses.ToListAsync();
        }

        public async Task<Expense?> GetExpenseById(int id)
        {
            return await _db.Expenses.FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task Delete(int id)
        {
            var expense = await GetExpenseById(id);
            if (expense != null)
            {
                _db.Expenses.Remove(expense);
                await _db.SaveChangesAsync();
            }
        }

        public async Task<Expense> Add(Expense expense)
        {
            _db.Expenses.Update(expense);
            await _db.SaveChangesAsync();
            return expense;
        }

        public async Task Update(int id, Expense expense)
        {
            if (id == expense.Id)
            {
                _db.Expenses.Update(expense);
                await _db.SaveChangesAsync();
            }
        }
    }
}
