using CEN4090L_Project.Data;
using CEN4090L_Project.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi.Enterprise;

namespace WebApi.Controllers
{
    public class SavingGoalsController : ControllerBase
    {
        private readonly SavingGoalEC _ec; // controller intermediary

        public SavingGoalsController(SavingGoalEC ec)
        {
            _ec = ec;
        }

        [HttpGet]
        public async Task<IEnumerable<SavingsGoal>> GetSavingsGoals()
        {
            return await _ec.GetSavingsGoals();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<SavingsGoal?>> GetSavingsGoalById(int id)
        {
            return await _ec.GetSavingsGoalById(id);
        }

        [HttpPost]
        public async Task<ActionResult<SavingsGoal>> Create([FromBody] SavingsGoal savingsGoal)
        {
            var created = await _ec.Add(savingsGoal);
            return CreatedAtAction(nameof(GetSavingsGoalById), new { id = savingsGoal.Id }, savingsGoal);
        }

        [HttpPut]
        public async Task Update(int id, [FromBody] SavingsGoal savingsGoal)
        {
            await _ec.Update(id, savingsGoal);
        }

        [HttpDelete("{id}")]
        public async Task Delete(int id)
        {
            await _ec.Delete(id);
        }
    }
}
