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
        public async Task<Budget> Create([FromBody] Budget budget)
        {
            return await _ec.Add(budget);
        }

        [HttpPut("{id}")]
        public async Task Update(int id, [FromBody] Budget budget)
        {
            return await _ec.Update(id,budget);
        }

        [HttpDelete("{id}")]
        public async Task Delete(int id)
        {
            await _ec.Delete(id);
        }
    }
}
