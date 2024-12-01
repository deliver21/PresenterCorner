using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PresenterCorner.Data;
using PresenterCorner.Models;
using System;

namespace PresenterCorner.Controllers
{
    [ApiController]
    [Route("api/presentation")]
    public class PresentationAPIController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public PresentationAPIController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("list")]
        public async Task<IActionResult> GetPresentations()
        {
            var presentations = await _context.Presentations.Include(p => p.Slides).ToListAsync();
            return Ok(presentations);
        }
        [HttpPost("{id}/join")]
        public async Task<IActionResult> JoinPresentation(int id, string nickname)
        {
            var presentation = await _context.Presentations
                .Include(p => p.Users)
                .FirstOrDefaultAsync(p => p.PresentationId == id);

            if (presentation == null)
                return NotFound("Presentation not found");

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Nickname == nickname);
            if (user == null)
                return BadRequest("User not found");

            if (presentation.Users.Any(u => u.Nickname == nickname))
                return BadRequest("User already joined this presentation");

            user.Role = "Viewer"; // Role stays Viewer by default
            user.PresentationId = id;
            await _context.SaveChangesAsync();

            return Ok(user);
        }

        [HttpPost("create-presentation")]
        public async Task<IActionResult> CreatePresentation(string title, string nickname)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Nickname == nickname);
            if (user == null)
                return BadRequest("User not found");

            // Create the presentation
            var presentation = new Presentation
            {
                Title = title,
                CreatorId = user.UserId,
                Slides = new List<Slide> { new Slide { Order = 1 } }
            };

            _context.Presentations.Add(presentation);
            await _context.SaveChangesAsync();

            // Update the user's role and presentation association
            user.Role = "Creator";
            user.PresentationId = presentation.PresentationId;
            await _context.SaveChangesAsync();

            return Ok(presentation);
        }      
    }
}
