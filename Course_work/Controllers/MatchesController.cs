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
            return await _context.Matches
                .Include(m => m.Tournament)
                .OrderByDescending(m => m.Match_date)
                .ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Match>> GetMatch(int id)
        {
            var match = await _context.Matches
                .Include(m => m.Tournament)
                .FirstOrDefaultAsync(m => m.ID_Matches == id);
            
            if (match == null) return NotFound();
            return match;
        }

        [HttpGet("ByTournament/{tournamentId}")]
        public async Task<ActionResult<IEnumerable<Match>>> GetMatchesByTournament(int tournamentId)
        {
            return await _context.Matches
                .Where(m => m.ID_Tournament == tournamentId)
                .OrderBy(m => m.Match_date)
                .ToListAsync();
        }

        [HttpGet("Upcoming")]
        public async Task<ActionResult<IEnumerable<Match>>> GetUpcomingMatches()
        {
            return await _context.Matches
                .Where(m => m.Status == "Запланирован" || m.Status == "Идет")
                .OrderBy(m => m.Match_date)
                .ToListAsync();
        }
    }
}