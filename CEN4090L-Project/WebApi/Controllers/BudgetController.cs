using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using CEN4090L_Project.Data;
using CEN4090L_Project.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using WebApi.Enterprise;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BudgetController : ControllerBase
    {
        private readonly BudgetEC _ec; // controller intermediary

        public BudgetController(BudgetEC ec)
        {
            _ec = ec;
        }

        [HttpGet("{id}")]
        public async Task<Budget?> GetBudgetById(int id)
        {
            return await _ec.GetBudgetById(id);
        }

        [HttpPost]
        public async Task<ActionResult<Budget>> Create([FromBody] Budget budget)
        {
            var created = await _ec.Add(budget);
            // returns 201 created response with location header
            return CreatedAtAction(nameof(GetBudgetById), new { id = created.Id }, created);
        }

        [HttpPut("{id}")]
        public async Task Update(int id, [FromBody] Budget budget)
        {
            await _ec.Update(id,budget);
        }

        [HttpDelete("{id}")]
        public async Task Delete(int id)
        {
            await _ec.Delete(id);
        }
    }
}
