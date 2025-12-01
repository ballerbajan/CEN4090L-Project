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
    public class BudgetController : ControllerBase
    {
        private readonly AppDbContext _dbContext;
        public BudgetController(AppDbContext dbContext) => _dbContext = dbContext;

        [HttpGet]
        public async Task<List<Budget>> Get()
        {
            return await _dbContext.Budgets.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Budget?>> GetById(int id)
        {
            return await _dbContext.Budgets.FirstOrDefaultAsync(x => x.Id == id);
        }

        [HttpPost]
        public async Task<ActionResult<Budget>> Create([FromBody] Budget budget)
        {
            //if(string.IsNullOrWhiteSpace(budget.Name) ||
            //    string.IsNullOrWhiteSpace(budget.Email) ||
            //    string.IsNullOrWhiteSpace(budget.Password) ||
            //    string.IsNullOrWhiteSpace(budget.Username))
            //{
            //    return BadRequest("Invalid Requests");
            //}
            await _dbContext.Budgets.AddAsync(budget);
            await _dbContext.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = budget.Id }, budget);
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
