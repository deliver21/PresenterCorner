using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PresenterCorner.Data;
using PresenterCorner.Models;
using System;

namespace PresenterCorner.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SlideElementAPIController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public SlideElementAPIController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddElement(int slideId, string type, string content, string position, string style)
        {
            var element = new SlideElement
            {
                SlideId = slideId,
                Type = type,
                Content = content,
                Position = position,
                Style = style,
                IsMovable = true
            };
            _context.SlideElements.Add(element);
            await _context.SaveChangesAsync();
            return Ok(element);
        }
    }

}
