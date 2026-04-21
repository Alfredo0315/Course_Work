using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Course_Work.Models;

namespace Course_Work.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NewsController : ControllerBase
    {
        private readonly EsportsDbContext _context;

        public NewsController(EsportsDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<News>>> GetNews()
        {
            try
            {
                var news = await _context.News
                    .OrderByDescending(n => n.Date_of_publication)
                    .ThenByDescending(n => n.Time_of_publication)
                    .ToListAsync();

                return Ok(news);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Ошибка загрузки новостей", error = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<News>> GetNews(int id)
        {
            try
            {
                var news = await _context.News.FindAsync(id);
                if (news == null) return NotFound(new { message = "Новость не найдена" });
                return Ok(news);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Ошибка загрузки новости", error = ex.Message });
            }
        }

        [HttpPost]
        public async Task<ActionResult<News>> CreateNews(News news)
        {
            try
            {
                _context.News.Add(news);
                await _context.SaveChangesAsync();
                return CreatedAtAction(nameof(GetNews), new { id = news.ID_News }, news);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Ошибка создания новости", error = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateNews(int id, News news)
        {
            news.ID_News = id;

            _context.Entry(news).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!NewsExists(id))
                {
                    return NotFound(new { message = "Новость не найдена" });
                }
                else
                {
                    throw;
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Ошибка обновления новости", error = ex.Message });
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteNews(int id)
        {
            try
            {
                var news = await _context.News.FindAsync(id);
                if (news == null)
                {
                    return NotFound(new { message = "Новость не найдена" });
                }

                _context.News.Remove(news);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Ошибка удаления новости", error = ex.Message });
            }
        }

        private bool NewsExists(int id)
        {
            return _context.News.Any(e => e.ID_News == id);
        }
    }
}