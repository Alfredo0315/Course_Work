using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Course_Work.Models;

namespace Course_Work.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TournamentsController : ControllerBase
    {
        private readonly EsportsDbContext _context;

        public TournamentsController(EsportsDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Tournament>>> GetTournaments()
        {
            try
            {
                var tournaments = await _context.Tournament
                    .OrderByDescending(t => t.Start_date)
                    .ToListAsync();
                
                return Ok(tournaments);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Ошибка загрузки турниров", error = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Tournament>> GetTournament(int id)
        {
            try
            {
                var tournament = await _context.Tournament
                    .Include(t => t.Teams)
                    .Include(t => t.Matches)
                    .FirstOrDefaultAsync(t => t.ID_Tournament == id);
                
                if (tournament == null) return NotFound();
                return Ok(tournament);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Ошибка", error = ex.Message });
            }
        }
    }
}