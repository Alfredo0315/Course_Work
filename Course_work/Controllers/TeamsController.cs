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
                var teams = await _context.TeamsTournaments
                    .Where(tt => tt.ID_Tournament == tournamentId)
                    .Select(tt => tt.Team)
                    .ToListAsync();

                return Ok(teams);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Ошибка загрузки команд турнира", error = ex.Message });
            }
        }

        [HttpPost]
        public async Task<ActionResult<Team>> CreateTeam(Team team)
        {
            try
            {
                _context.Teams.Add(team);
                await _context.SaveChangesAsync();
                return CreatedAtAction(nameof(GetTeam), new { id = team.ID_Teams }, team);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Ошибка создания команды", error = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTeam(int id, Team team)
        {
            team.ID_Teams = id;

            _context.Entry(team).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TeamExists(id))
                {
                    return NotFound(new { message = "Команда не найдена" });
                }
                else
                {
                    throw;
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Ошибка обновления команды", error = ex.Message });
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTeam(int id)
        {
            try
            {
                var team = await _context.Teams.FindAsync(id);
                if (team == null)
                {
                    return NotFound(new { message = "Команда не найдена" });
                }

                _context.Teams.Remove(team);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Ошибка удаления команды", error = ex.Message });
            }
        }

        private bool TeamExists(int id)
        {
            return _context.Teams.Any(e => e.ID_Teams == id);
        }
    }
}