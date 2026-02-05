using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Course_Work.Models;

namespace Course_Work.Controllers
{
    /// <summary>
    /// API контроллер для управления игроками
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class PlayersController : ControllerBase
    {
        private readonly EsportsDbContext _context;

        public PlayersController(EsportsDbContext context)
        {
            _context = context;
        }

        // GET: api/Players
        /// <summary>
        /// Получить список всех игроков
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Player>>> GetPlayers()
        {
            return await _context.Players
                .Include(p => p.Team)
                .Include(p => p.PlayersGames)
                    .ThenInclude(pg => pg.Game)
                .ToListAsync();
        }

        // GET: api/Players/5
        /// <summary>
        /// Получить игрока по ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<Player>> GetPlayer(int id)
        {
            var player = await _context.Players
                .Include(p => p.Team)
                .Include(p => p.PlayersGames)
                    .ThenInclude(pg => pg.Game)
                .FirstOrDefaultAsync(p => p.ID_Players == id);

            if (player == null)
            {
                return NotFound();
            }

            return player;
        }

        // GET: api/Players/ByTeam/5
        /// <summary>
        /// Получить игроков по команде
        /// </summary>
        [HttpGet("ByTeam/{teamId}")]
        public async Task<ActionResult<IEnumerable<Player>>> GetPlayersByTeam(int teamId)
        {
            var players = await _context.Players
                .Include(p => p.Team)
                .Where(p => p.ID_Teams == teamId)
                .ToListAsync();

            return Ok(players);
        }

        // GET: api/Players/ByCountry/Ukraine
        /// <summary>
        /// Получить игроков по стране
        /// </summary>
        [HttpGet("ByCountry/{country}")]
        public async Task<ActionResult<IEnumerable<Player>>> GetPlayersByCountry(string country)
        {
            var players = await _context.Players
                .Include(p => p.Team)
                .Where(p => p.Country == country)
                .ToListAsync();

            return Ok(players);
        }

        // GET: api/Players/TopByPrize
        /// <summary>
        /// Получить топ игроков по призовому фонду
        /// </summary>
        [HttpGet("TopByPrize")]
        public async Task<ActionResult<IEnumerable<Player>>> GetTopPlayersByPrize(int count = 10)
        {
            var players = await _context.Players
                .Include(p => p.Team)
                .OrderByDescending(p => p.Prize_pool)
                .Take(count)
                .ToListAsync();

            return Ok(players);
        }

        // POST: api/Players
        /// <summary>
        /// Создать нового игрока
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<Player>> PostPlayer(Player player)
        {
            _context.Players.Add(player);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetPlayer), new { id = player.ID_Players }, player);
        }

        // PUT: api/Players/5
        /// <summary>
        /// Обновить информацию об игроке
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPlayer(int id, Player player)
        {
            if (id != player.ID_Players)
            {
                return BadRequest();
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
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // DELETE: api/Players/5
        /// <summary>
        /// Удалить игрока
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePlayer(int id)
        {
            var player = await _context.Players.FindAsync(id);
            if (player == null)
            {
                return NotFound();
            }

            _context.Players.Remove(player);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PlayerExists(int id)
        {
            return _context.Players.Any(e => e.ID_Players == id);
        }
    }
}