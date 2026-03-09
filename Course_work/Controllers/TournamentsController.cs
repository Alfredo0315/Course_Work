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
        public async Task<ActionResult<IEnumerable<object>>> GetTournaments()
        {
            try
            {
                var tournaments = await _context.Tournaments
                    .Include(t => t.Game)
                    .OrderByDescending(t => t.Start_date)
                    .ToListAsync();
        
                var result = tournaments.Select(t => new
                {
                    t.ID_Tournament,
                    t.Name,
                    t.Start_date,
                    t.End_date,
                    t.Prize_pool,
                    t.Location_of_the_event,
                    t.Description,
                    GameName = t.Game?.Name ?? "N/A",
                    Genre = t.Game?.Jenre ?? "N/A",
                    Platform = t.Game?.Platform ?? "N/A"
                });
        
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Ошибка загрузки турниров", error = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<object>> GetTournament(int id)
        {
            try
            {
                var tournament = await _context.Tournaments
                    .Where(t => t.ID_Tournament == id)
                    .Select(t => new
                    {
                        t.ID_Tournament,
                        t.Name,
                        t.Start_date,
                        t.End_date,
                        t.Prize_pool,
                        t.Location_of_the_event,
                        t.Description,
                        GameName = t.Game != null ? t.Game.Name : "N/A",
                        Genre = t.Game != null ? t.Game.Jenre : "N/A",
                        Platform = t.Game != null ? t.Game.Platform : "N/A",
                        Teams = _context.TeamsTournaments
                            .Where(tt => tt.ID_Tournament == id)
                            .Select(tt => new
                            {
                                tt.ID_Teams,
                                tt.Team.Name,
                                tt.Team.Country,
                                tt.Team.Prize_pool
                            })
                            .ToList(),
                        Matches = t.Matches.Select(m => new
                        {
                            m.ID_Matches,
                            m.Match_date,
                            Match_Time = m.Match_time.ToString(@"hh\:mm"),
                            m.Score,
                            m.Status
                        }).ToList()
                    })
                    .FirstOrDefaultAsync();

                if (tournament == null) 
                    return NotFound(new { message = "Турнир не найден" });
                    
                return Ok(tournament);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Ошибка", error = ex.Message });
            }
        }
    }
}