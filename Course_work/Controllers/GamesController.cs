using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Course_Work.Models;

namespace Course_Work.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GamesController : ControllerBase
    {
        private readonly EsportsDbContext _context;

        public GamesController(EsportsDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Game>>> GetGames()
        {
            try
            {
                var games = await _context.Games.ToListAsync();
                return Ok(games);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Ошибка загрузки игр", error = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Game>> GetGame(int id)
        {
            try
            {
                var game = await _context.Games.FindAsync(id);
                if (game == null) return NotFound(new { message = "Игра не найдена" });
                return Ok(game);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Ошибка загрузки игры", error = ex.Message });
            }
        }
    }
}