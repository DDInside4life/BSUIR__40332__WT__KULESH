using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using KULESH.API.Data;
using KULESH.Domain.Entities;
using KULESH.Domain.Models;

namespace KULESH.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FootballTeamsController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public FootballTeamsController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        // GET: api/FootballTeams
        // Optional query parameters:
        //   categoryId - filter by CategoryId
        //   category - filter by category NormalizedName
        [HttpGet]
        public async Task<ActionResult<ResponseData<List<FootballTeam>>>> GetFootballTeams([FromQuery] int? categoryId = null, [FromQuery] string? category = null)
        {
            try
            {
                // Include Category navigation property
                var query = _context.FootballTeams.Include(t => t.Category).AsQueryable();

                if (categoryId.HasValue)
                {
                    query = query.Where(t => t.CategoryId == categoryId.Value);
                }

                if (!string.IsNullOrWhiteSpace(category))
                {
                    query = query.Where(t => t.Category != null && t.Category.NormalizedName.Equals(category, StringComparison.OrdinalIgnoreCase));
                }

                var list = await query.ToListAsync();

                return Ok(ResponseData<List<FootballTeam>>.OK(list));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ResponseData<List<FootballTeam>>.Error(ex.Message));
            }
        }

        // GET: api/FootballTeams/5
        [HttpGet("{id}")]
        public async Task<ActionResult<FootballTeam>> GetFootballTeam(int id)
        {
            var footballTeam = await _context.FootballTeams.FindAsync(id);

            if (footballTeam == null)
            {
                return NotFound();
            }

            return footballTeam;
        }

        // PUT: api/FootballTeams/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutFootballTeam(int id, FootballTeam footballTeam)
        {
            if (id != footballTeam.Id)
            {
                return BadRequest();
            }

            _context.Entry(footballTeam).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FootballTeamExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/FootballTeams
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<FootballTeam>> PostFootballTeam(FootballTeam footballTeam)
        {
            _context.FootballTeams.Add(footballTeam);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetFootballTeam", new { id = footballTeam.Id }, footballTeam);
        }

        // DELETE: api/FootballTeams/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFootballTeam(int id)
        {
            var footballTeam = await _context.FootballTeams.FindAsync(id);
            if (footballTeam == null)
            {
                return NotFound();
            }

            _context.FootballTeams.Remove(footballTeam);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // POST: api/FootballTeams/{id}/image
        // Upload an image for the specified football team. Expects multipart/form-data with file field named "file".
        [HttpPost("{id}/image")]
        public async Task<IActionResult> UploadImage(int id, IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest(ResponseData<string>.Error("No file uploaded"));

            var footballTeam = await _context.FootballTeams.FindAsync(id);
            if (footballTeam == null)
                return NotFound(ResponseData<string>.Error("Team not found"));

            // Ensure wwwroot/Images exists
            var webRoot = _env.WebRootPath;
            if (string.IsNullOrEmpty(webRoot))
            {
                webRoot = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
            }

            var imagesFolder = Path.Combine(webRoot, "Images");
            Directory.CreateDirectory(imagesFolder);

            var ext = Path.GetExtension(file.FileName);
            var fileName = Guid.NewGuid().ToString() + ext;
            var filePath = Path.Combine(imagesFolder, fileName);

            using (var stream = System.IO.File.Create(filePath))
            {
                await file.CopyToAsync(stream);
            }

            // Save relative url to the entity
            footballTeam.Image = "/Images/" + fileName;
            _context.FootballTeams.Update(footballTeam);
            await _context.SaveChangesAsync();

            return Ok(ResponseData<FootballTeam>.OK(footballTeam));
        }

        private bool FootballTeamExists(int id)
        {
            return _context.FootballTeams.Any(e => e.Id == id);
        }
    }
}
