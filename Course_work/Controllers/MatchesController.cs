using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Course_Work.Models;

namespace Course_Work.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MatchesController : ControllerBase
    {
        private readonly EsportsDbContext _context;

        public MatchesController(EsportsDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<object>>> GetMatches()
        {
            try
            {
                var matches = await _context.Matches
                    .OrderByDescending(m => m.Match_date)
                    .Select(m => new
                    {
                        m.ID_Matches,
                        m.Match_date,
                        Match_Time = m.Match_time.ToString(@"hh\:mm"),
                        m.Score,
                        m.Status,
                        m.ID_Tournament,
                        TournamentName = m.Tournament.Name, // Название турнира
                        Teams = _context.TeamsMatches
                            .Where(tm => tm.ID_Matches == m.ID_Matches)
                            .Select(tm => new
                            {
                                tm.ID_Teams,
                                tm.Team_Side,
                                Name = tm.Team.Name,
                                Country = tm.Team.Country
                            })
                            .ToList()
                    })
                    .ToListAsync();

                return Ok(matches);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Ошибка загрузки матчей", details = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<object>> GetMatch(int id)
        {
            try
            {
                var match = await _context.Matches
                    .Where(m => m.ID_Matches == id)
                    .Select(m => new
                    {
                        m.ID_Matches,
                        m.Match_date,
                        Match_Time = m.Match_time.ToString(@"hh\:mm"),
                        m.Score,
                        m.Status,
                        m.ID_Tournament,
                        TournamentName = m.Tournament.Name,
                        Teams = _context.TeamsMatches
                            .Where(tm => tm.ID_Matches == m.ID_Matches)
                            .Select(tm => new
                            {
                                tm.ID_Teams,
                                tm.Team_Side,
                                Name = tm.Team.Name,
                                Country = tm.Team.Country,
                                Prize_pool = tm.Team.Prize_pool
                            })
                            .ToList()
                    })
                    .FirstOrDefaultAsync();

                if (match == null)
                    return NotFound(new { error = $"Матч с ID {id} не найден" });

                return Ok(match);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Ошибка загрузки матча", details = ex.Message });
            }
        }

        [HttpGet("ByTournament/{tournamentId}")]
        public async Task<ActionResult<IEnumerable<object>>> GetMatchesByTournament(int tournamentId)
        {
            try
            {
                var matches = await _context.Matches
                    .Where(m => m.ID_Tournament == tournamentId)
                    .OrderByDescending(m => m.Match_date)
                    .Select(m => new
                    {
                        m.ID_Matches,
                        m.Match_date,
                        Match_Time = m.Match_time.ToString(@"hh\:mm"),
                        m.Score,
                        m.Status,
                        m.ID_Tournament,
                        TournamentName = m.Tournament.Name,
                        Teams = _context.TeamsMatches
                            .Where(tm => tm.ID_Matches == m.ID_Matches)
                            .Select(tm => new
                            {
                                tm.ID_Teams,
                                tm.Team_Side,
                                Name = tm.Team.Name
                            })
                            .ToList()
                    })
                    .ToListAsync();

                return Ok(matches);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Ошибка загрузки матчей турнира", details = ex.Message });
            }
        }

        [HttpGet("Upcoming")]
        public async Task<ActionResult<IEnumerable<object>>> GetUpcomingMatches()
        {
            try
            {
                var matches = await _context.Matches
                    .Include(m => m.Tournament)
                    .Where(m => m.Status == "Запланирован" || m.Status == "Идет")
                    .OrderBy(m => m.Match_date)
                    .Take(10)
                    .ToListAsync();

                var result = matches.Select(m => new
                {
                    m.ID_Matches,
                    m.Match_date,
                    Match_Time = m.Match_time.ToString(@"hh\:mm"),
                    m.Score,
                    m.Status,
                    m.ID_Tournament,
                    TournamentName = m.Tournament?.Name ?? "N/A",
                    Teams = _context.TeamsMatches
                        .Where(tm => tm.ID_Matches == m.ID_Matches)
                        .Select(tm => new
                        {
                            tm.ID_Teams,
                            tm.Team_Side,
                            Name = tm.Team.Name
                        })
                        .ToList()
                });

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Ошибка загрузки предстоящих матчей", details = ex.Message });
            }
        }
        }
    }
    