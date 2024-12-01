using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using PresenterCorner.Data;
using PresenterCorner.Models;

namespace PresenterCorner.Controllers
{
    [Route("api/user")]
    public class UserAPIController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public UserAPIController(ApplicationDbContext context)
        {
            _context = context;
        }
        [HttpGet("{id}/users")]
        public async Task<IActionResult> GetUsers(int id)
        {
            var users = await _context.Users
                .Where(u => u.PresentationId == id)
                .Select(u => new { u.Nickname, u.Role })
                .ToListAsync();

            return Ok(users);
        }

        [HttpPost("create-user")]
        public async Task<IActionResult> CreateUser([FromBody] string nickname)
        {
            if(nickname == null)
            {
                return BadRequest();
            }
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Nickname == nickname);
            if (user == null)
            {
                 user = new User
                {
                    Nickname = nickname,
                    Role = "Viewer", // Default role
                    PresentationId = 0 // Not assigned to any presentation yet
                };
                _context.Users.Add(user);
                await _context.SaveChangesAsync();
            }

            return Ok(user);
        }
    }
}
