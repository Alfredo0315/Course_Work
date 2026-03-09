using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Course_Work.Models;

namespace Course_Work.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlayersController : ControllerBase
    {
        private readonly EsportsDbContext _context;

        public PlayersController(EsportsDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<object>>> GetPlayers()
        {
            try
            {
                var players = await _context.Players
                    .Include(p => p.Team)
                    .OrderByDescending(p => p.Prize_pool)
                    .ToListAsync();
                var result = players.Select(p => new
                {
                    p.ID_Players,
                    p.Nickname,
                    p.Name,
                    p.Surname,
                    p.Country,
                    p.Date_of_birth,
                    p.Prize_pool,
                    p.ID_Teams,
                    TeamName = p.Team?.Name ?? "Без команды"
                });
        
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Ошибка загрузки игроков", error = ex.Message });
            }
        }
        
        [HttpGet("{id}")]
        public async Task<ActionResult<object>> GetPlayer(int id)
        {
            try
            {
                var player = await _context.Players
                    .Where(p => p.ID_Players == id)
                    .Select(p => new
                    {
                        p.ID_Players,
                        p.Nickname,
                        p.Name,
                        p.Surname,
                        p.Country,
                        p.Date_of_birth,
                        p.Prize_pool,
                        p.ID_Teams,
                        TeamName = p.Team != null ? p.Team.Name : "Без команды"
                    })
                    .FirstOrDefaultAsync();
                
                if (player == null)
                {
                    return NotFound(new { message = "Игрок не найден" });
                }

                return Ok(player);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Ошибка загрузки игрока", error = ex.Message });
            }
        }

        [HttpGet("ByTeam/{teamId}")]
        public async Task<ActionResult<IEnumerable<Player>>> GetPlayersByTeam(int teamId)
        {
            try
            {
                var players = await _context.Players
                    .Where(p => p.ID_Teams == teamId)
                    .ToListAsync();
                
                return Ok(players);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Ошибка загрузки игроков команды", error = ex.Message });
            }
        }

        [HttpGet("ByCountry/{country}")]
        public async Task<ActionResult<IEnumerable<Player>>> GetPlayersByCountry(string country)
        {
            try
            {
                var players = await _context.Players
                    .Where(p => p.Country == country)
                    .ToListAsync();
                
                return Ok(players);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Ошибка загрузки игроков по стране", error = ex.Message });
            }
        }

        [HttpGet("TopByPrize")]
        public async Task<ActionResult<IEnumerable<object>>> GetTopPlayersByPrize([FromQuery] int count = 10)
        {
            try
            {
                var players = await _context.Players
                    .OrderByDescending(p => p.Prize_pool)
                    .Take(count)
                    .Select(p => new
                    {
                        p.ID_Players,
                        p.Nickname,
                        p.Name,
                        p.Surname,
                        p.Country,
                        p.Prize_pool,
                        TeamName = p.Team != null ? p.Team.Name : "Без команды"
                    })
                    .ToListAsync();
                
                return Ok(players);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Ошибка загрузки топ игроков", error = ex.Message });
            }
        }

        [HttpPost]
        public async Task<ActionResult<Player>> CreatePlayer(Player player)
        {
            try
            {
                _context.Players.Add(player);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetPlayer), new { id = player.ID_Players }, player);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Ошибка создания игрока", error = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePlayer(int id, Player player)
        {
            if (id != player.ID_Players)
            {
                return BadRequest(new { message = "ID не совпадает" });
            }

            _context.Entry(player).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PlayerExists(id))
                {
                    return NotFound(new { message = "Игрок не найден" });
                }
                else
                {
                    throw;
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Ошибка обновления игрока", error = ex.Message });
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePlayer(int id)
        {
            try
            {
                var player = await _context.Players.FindAsync(id);
                if (player == null)
                {
                    return NotFound(new { message = "Игрок не найден" });
                }

                _context.Players.Remove(player);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Ошибка удаления игрока", error = ex.Message });
            }
        }

        private bool PlayerExists(int id)
        {
            return _context.Players.Any(e => e.ID_Players == id);
        }
    }
}