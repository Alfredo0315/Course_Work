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
    }
}