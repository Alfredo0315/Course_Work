using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Course_Work.Models;

namespace Course_Work.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeamsController : ControllerBase
    {
        private readonly EsportsDbContext _context;

        public TeamsController(EsportsDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Team>>> GetTeams()
        {
            try
            {
                var teams = await _context.Teams
                    .OrderByDescending(t => t.Prize_pool)
                    .ToListAsync();
                
                return Ok(teams);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Ошибка загрузки команд", error = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Team>> GetTeam(int id)
        {
            try
            {
                var team = await _context.Teams
                    .Include(t => t.Players)
                    .FirstOrDefaultAsync(t => t.ID_Teams == id);
                
                if (team == null) return NotFound(new { message = "Команда не найдена" });
                return Ok(team);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Ошибка загрузки команды", error = ex.Message });
            }
        }

        [HttpGet("ByTournament/{tournamentId}")]
        public async Task<ActionResult<IEnumerable<Team>>> GetTeamsByTournament(int tournamentId)
        {
            try
            {
                var teams = await _context.Teams
                    .Where(t => t.ID_Tournament == tournamentId)
                    .ToListAsync();
                
                return Ok(teams);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Ошибка загрузки команд турнира", error = ex.Message });
            }
        }
    }
}
