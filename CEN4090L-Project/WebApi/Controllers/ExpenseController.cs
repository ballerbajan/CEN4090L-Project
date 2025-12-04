using CEN4090L_Project.Data;
using CEN4090L_Project.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi.Enterprise;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExpenseController : ControllerBase
    {
        private readonly ExpenseEC _ec; // controller intermediary


        public ExpenseController(ExpenseEC ec) {
            _ec = ec;
        }

        [HttpGet]
        public async Task<IEnumerable<Expense>> GetExpenses()
        {
            return await _ec.GetExpenses();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Expense?>> GetExpenseById(int id)
        {
            return await _ec.GetExpenseById(id);
        }


        [HttpPost]
        public async Task<ActionResult<Expense>> Create([FromBody] Expense expense)
        {
            var created = await _ec.Add(expense);
            return CreatedAtAction(nameof(GetExpenseById), new { id = created.Id }, created);
        }

        [HttpPut]
        public async Task Update(int id, [FromBody] Expense expense)
        {
            await _ec.Update(id, expense);
        }
    }
}