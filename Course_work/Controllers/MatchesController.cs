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
                        TournamentName = m.Tournament.Name,
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

        [HttpPost]
        public async Task<ActionResult<Match>> CreateMatch(MatchDto dto)
        {
            try
            {
                var match = new Match
                {
                    ID_Tournament = dto.ID_Tournament,
                    Match_date = dto.Match_date,
                    Match_time = TimeSpan.TryParse(dto.Match_time, out var t) ? t : TimeSpan.Zero,
                    Status = dto.Status ?? "Запланирован",
                    Score = dto.Score
                };
                _context.Matches.Add(match);
                await _context.SaveChangesAsync();
                return CreatedAtAction(nameof(GetMatch), new { id = match.ID_Matches }, match);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Ошибка создания матча", error = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMatch(int id, MatchDto dto)
        {
            try
            {
                var match = await _context.Matches.FindAsync(id);
                if (match == null)
                    return NotFound(new { message = "Матч не найден" });

                match.ID_Tournament = dto.ID_Tournament;
                match.Match_date = dto.Match_date;
                match.Match_time = TimeSpan.TryParse(dto.Match_time, out var t) ? t : TimeSpan.Zero;
                match.Status = dto.Status ?? match.Status;
                match.Score = dto.Score;

                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MatchExists(id))
                    return NotFound(new { message = "Матч не найден" });
                throw;
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Ошибка обновления матча", error = ex.Message });
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMatch(int id)
        {
            try
            {
                var match = await _context.Matches.FindAsync(id);
                if (match == null)
                {
                    return NotFound(new { message = "Матч не найден" });
                }

                _context.Matches.Remove(match);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Ошибка удаления матча", error = ex.Message });
            }
        }

        private bool MatchExists(int id)
        {
            return _context.Matches.Any(e => e.ID_Matches == id);
        }
    }

    public class MatchDto
    {
        public int ID_Tournament { get; set; }
        public DateTime Match_date { get; set; }
        public string? Match_time { get; set; }
        public string? Status { get; set; }
        public string? Score { get; set; }
    }
}