using CEN4090L_Project.Data;
using CEN4090L_Project.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WebApi.Controllers
{
    public class SavingGoalsController : ControllerBase
    {
        private readonly AppDbContext _dbContext;
        public SavingGoalsController(AppDbContext dbContext) => _dbContext = dbContext;

        [HttpGet]
        public async Task<List<SavingsGoal>> Get()
        {
            return await _dbContext.SavingsGoals.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<SavingsGoal?>> GetById(int id)
        {
            return await _dbContext.SavingsGoals.FirstOrDefaultAsync(x => x.Id == id);
        }

        [HttpPost]
        public async Task<ActionResult<SavingsGoal>> Create([FromBody] SavingsGoal savingsGoal)
        {
            //if(string.IsNullOrWhiteSpace(budget.Name) ||
            //    string.IsNullOrWhiteSpace(budget.Email) ||
            //    string.IsNullOrWhiteSpace(budget.Password) ||
            //    string.IsNullOrWhiteSpace(budget.Username))
            //{
            //    return BadRequest("Invalid Requests");
            //}
            await _dbContext.SavingsGoals.AddAsync(savingsGoal);
            await _dbContext.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = savingsGoal.Id }, savingsGoal);
        }

        [HttpPut]
        public async Task<ActionResult> Update([FromBody] SavingsGoal savingsGoal)
        {
            //if (user.Id == 0 ||
            //   string.IsNullOrWhiteSpace(user.Name) ||
            //   string.IsNullOrWhiteSpace(user.Email) ||
            //   string.IsNullOrWhiteSpace(user.Password) ||
            //   string.IsNullOrWhiteSpace(user.Username))
            //{
            //    return BadRequest("Invalid Requests");
            //}

            await _dbContext.SavingsGoals.AddAsync(savingsGoal);
            await _dbContext.SaveChangesAsync();

            return Ok();
        }

        //[HttpDelete("{id}")]
        //public async Task<ActionResult> Delete(int id)
        //{
        //    var budget = await GetById(id);
        //    if (budget is null)
        //    {
        //        return NotFound();
        //    }
        //    _dbContext.Users.Remove(budget);
        //    await _dbContext.SaveChangesAsync();
        //    return Ok();
        //}


    }
}
