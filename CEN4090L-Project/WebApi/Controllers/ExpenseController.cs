using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using CEN4090L_Project.Data;
using CEN4090L_Project.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExpenseController : ControllerBase
    {
        private readonly AppDbContext _dbContext;

        public ExpenseController(AppDbContext dbContext) => _dbContext = dbContext;

        [HttpGet]
        public async Task<List<Expense>> Get()
        {
            return await _dbContext.Expenses.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Expense?>> GetById(int id)
        {
            return await _dbContext.Expenses.FirstOrDefaultAsync(x => x.Id == id);
        }


        [HttpPost]
        public async Task<ActionResult<Expense>> Create([FromBody] Expense expense)
        {
            //if(string.IsNullOrWhiteSpace(budget.Name) ||
            //    string.IsNullOrWhiteSpace(budget.Email) ||
            //    string.IsNullOrWhiteSpace(budget.Password) ||
            //    string.IsNullOrWhiteSpace(budget.Username))
            //{
            //    return BadRequest("Invalid Requests");
            //}
            await _dbContext.Expenses.AddAsync(expense);
            await _dbContext.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = expense.Id }, expense);
        }

        [HttpPut]
        public async Task<ActionResult> Update([FromBody] Budget budget)
        {
            //if (user.Id == 0 ||
            //   string.IsNullOrWhiteSpace(user.Name) ||
            //   string.IsNullOrWhiteSpace(user.Email) ||
            //   string.IsNullOrWhiteSpace(user.Password) ||
            //   string.IsNullOrWhiteSpace(user.Username))
            //{
            //    return BadRequest("Invalid Requests");
            //}

            await _dbContext.Budgets.AddAsync(budget);
            await _dbContext.SaveChangesAsync();

            return Ok();
        }

        ////[HttpDelete("{id}")]





    }
}
