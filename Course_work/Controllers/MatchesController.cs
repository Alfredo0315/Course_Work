using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Course_Work.Models;

namespace Course_Work.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MatchesController : ControllerBase
    {
        private readonly EsportsDbContext _context;

        public MatchesController(EsportsDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Match>>> GetMatches()
        {
            try
            {
                var matches = await _context.Matches
                    .OrderByDescending(m => m.Match_date)
                    .ToListAsync();
                
                return Ok(matches);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Ошибка загрузки матчей", error = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Match>> GetMatch(int id)
        {
            try
            {
                var match = await _context.Matches.FindAsync(id);
                if (match == null) return NotFound(new { message = "Матч не найден" });
                return Ok(match);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Ошибка загрузки матча", error = ex.Message });
            }
        }

        [HttpGet("ByTournament/{tournamentId}")]
        public async Task<ActionResult<IEnumerable<Match>>> GetMatchesByTournament(int tournamentId)
        {
            try
            {
                var matches = await _context.Matches
                    .Where(m => m.ID_Tournament == tournamentId)
                    .OrderBy(m => m.Match_date)
                    .ToListAsync();
                
                return Ok(matches);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Ошибка загрузки матчей турнира", error = ex.Message });
            }
        }

        [HttpGet("Upcoming")]
        public async Task<ActionResult<IEnumerable<Match>>> GetUpcomingMatches()
        {
            try
            {
                var matches = await _context.Matches
                    .Where(m => m.Status == "Запланирован" || m.Status == "Идет")
                    .OrderBy(m => m.Match_date)
                    .ToListAsync();
                
                return Ok(matches);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Ошибка загрузки предстоящих матчей", error = ex.Message });
            }
        }
    }
}