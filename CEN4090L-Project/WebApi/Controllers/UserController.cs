using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using CEN4090L_Project.Data;
using CEN4090L_Project.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using WebApi.Enterprise;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserEC _ec; // controller intermediary
        public UserController(UserEC ec)
        {
            _ec = ec;
        }

        [HttpGet]
        public async Task<IEnumerable<User>> GetUsers()
        {
            return await _ec.GetUsers();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<User?>> GetUserById(int id)
        {
            return await _ec.GetUserById(id);
        }

        [HttpPost]
        public async Task<ActionResult<User>> Create([FromBody] User user)
        {
            var created = await _ec.Add(user);
            return CreatedAtAction(nameof(GetUserById), new { id = user.Id }, user);
        }

        [HttpPut]
        public async Task Update(int id, [FromBody] User user)
        {
            await _ec.Update(id, user);
        }

        [HttpDelete("{id}")]
        public async Task Delete(int id)
        {
            await _ec.Delete(id);
        }
    }
}
