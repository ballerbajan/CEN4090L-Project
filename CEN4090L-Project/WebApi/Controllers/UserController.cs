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
    public class UserController : ControllerBase
    {
        private readonly AppDbContext _dbContext;
        public UserController(AppDbContext dbContext) => _dbContext = dbContext;

        [HttpGet]
        public async Task<List<User>> Get()
        {
            return await _dbContext.Users.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<User?>> GetById(int id)
        {
            return await _dbContext.Users.FirstOrDefaultAsync(x => x.Id == id);

        }

        [HttpPost]
        public async Task<ActionResult<User>> Create([FromBody] User user)
        {
            if (string.IsNullOrWhiteSpace(user.Name) ||
                string.IsNullOrWhiteSpace(user.Email) ||
                string.IsNullOrWhiteSpace(user.Password) ||
                string.IsNullOrWhiteSpace(user.Username))
            {
                return BadRequest("Invalid Requests");
            }

            await _dbContext.Users.AddAsync(user);
            await _dbContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = user.Id }, user);
        }

        [HttpPut]
        public async Task<ActionResult> Update([FromBody] User user)
        {
            if (user.Id == 0 || 
               string.IsNullOrWhiteSpace(user.Name) ||
               string.IsNullOrWhiteSpace(user.Email) ||
               string.IsNullOrWhiteSpace(user.Password) ||
               string.IsNullOrWhiteSpace(user.Username))
            {
                return BadRequest("Invalid Requests");
            }

            await _dbContext.Users.AddAsync(user);
            await _dbContext.SaveChangesAsync();

            return Ok();
        }

        //[HttpDelete("{id}")]
        //public async Task<ActionResult> Delete(int id)
        //{
        //    var user = await GetById(id);
        //    if (user is null)
        //    {
        //        return NotFound();
        //    }
        //    _dbContext.Users.Remove(user);
        //    await _dbContext.SaveChangesAsync();
        //    return Ok();
        //}



    }
}
