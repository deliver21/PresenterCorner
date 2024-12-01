using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PresenterCorner.Data;
using PresenterCorner.Models;
using System;

namespace PresenterCorner.Controllers
{
    [ApiController]
    [Route("api/slide")]
    public class SlideAPIController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public SlideAPIController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddSlide(int presentationId)
        {
            var slide = new Slide
            {
                PresentationId = presentationId,
                Order = _context.Slides.Count(s => s.PresentationId == presentationId) + 1
            };
            _context.Slides.Add(slide);
            await _context.SaveChangesAsync();
            return Ok(slide);
        }
    }

}
