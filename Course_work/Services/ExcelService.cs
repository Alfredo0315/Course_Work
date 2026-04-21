using Course_Work.Models;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Drawing;

namespace Course_Work.Services
{
    public class ExcelService
    {
        private readonly EsportsDbContext _context;

        public ExcelService(EsportsDbContext context)
        {
            _context = context;
        }

        public async Task<byte[]> GetExcelAsync()
        {
            // Получаем данные из БД
            var tournaments = await _context.Tournaments
                .OrderByDescending(t => t.Start_date)
                .Take(50)
                .ToListAsync();

            var matches = await _context.Matches
                .OrderByDescending(m => m.Match_date)
                .Take(100)
                .ToListAsync();

            var teams = await _context.Teams
                .OrderByDescending(t => t.Prize_pool)
                .Take(50)
                .ToListAsync();

            var players = await _context.Players
                .OrderByDescending(p => p.Prize_pool)
                .Take(50)
                .ToListAsync();

            // Создаем Excel пакет
            using (var package = new ExcelPackage())
            {
                // ========================================
                // ЛИСТ 1: ТУРНИРЫ
                // ========================================
                var tournamentSheet = package.Workbook.Worksheets.Add("Турниры");

                // Заголовки
                tournamentSheet.Cells[1, 1].Value = "ID";
                tournamentSheet.Cells[1, 2].Value = "Название";
                tournamentSheet.Cells[1, 3].Value = "Дата начала";
                tournamentSheet.Cells[1, 4].Value = "Дата окончания";
                tournamentSheet.Cells[1, 5].Value = "Призовой фонд ($)";
                tournamentSheet.Cells[1, 6].Value = "Место проведения";

                // Форматирование заголовков
                using (var range = tournamentSheet.Cells[1, 1, 1, 6])
                {
                    range.Style.Font.Bold = true;
                    range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(173, 216, 230)); // LightBlue
                    range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    range.Style.Border.BorderAround(ExcelBorderStyle.Thin);
                }

                // Заполняем данными
                for (int i = 0; i < tournaments.Count; i++)
                {
                    var t = tournaments[i];
                    int row = i + 2;

                    tournamentSheet.Cells[row, 1].Value = t.ID_Tournament;
                    tournamentSheet.Cells[row, 2].Value = t.Name;
                    tournamentSheet.Cells[row, 3].Value = t.Start_date.ToString("dd.MM.yyyy");
                    tournamentSheet.Cells[row, 4].Value = t.End_date?.ToString("dd.MM.yyyy") ?? "";
                    tournamentSheet.Cells[row, 5].Value = t.Prize_pool ?? 0;
                    tournamentSheet.Cells[row, 6].Value = t.Location_of_the_event ?? "";
                }

                // Автоподбор ширины
                tournamentSheet.Cells.AutoFitColumns();

                // ========================================
                // ЛИСТ 2: МАТЧИ
                // ========================================
                var matchSheet = package.Workbook.Worksheets.Add("Матчи");

                // Заголовки
                matchSheet.Cells[1, 1].Value = "ID";
                matchSheet.Cells[1, 2].Value = "Дата";
                matchSheet.Cells[1, 3].Value = "Счёт";
                matchSheet.Cells[1, 4].Value = "Статус";
                matchSheet.Cells[1, 5].Value = "ID Турнира";

                // Форматирование заголовков
                using (var range = matchSheet.Cells[1, 1, 1, 5])
                {
                    range.Style.Font.Bold = true;
                    range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(144, 238, 144)); // LightGreen
                    range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    range.Style.Border.BorderAround(ExcelBorderStyle.Thin);
                }

                // Заполняем данными
                for (int i = 0; i < matches.Count; i++)
                {
                    var m = matches[i];
                    int row = i + 2;

                    matchSheet.Cells[row, 1].Value = m.ID_Matches;
                    matchSheet.Cells[row, 2].Value = m.Match_date.ToString("dd.MM.yyyy");
                    matchSheet.Cells[row, 3].Value = m.Score ?? "";
                    matchSheet.Cells[row, 4].Value = m.Status;
                    matchSheet.Cells[row, 5].Value = m.ID_Tournament;
                }

                matchSheet.Cells.AutoFitColumns();

                // ========================================
                // ЛИСТ 3: КОМАНДЫ
                // ========================================
                var teamSheet = package.Workbook.Worksheets.Add("Команды");

                // Заголовки
                teamSheet.Cells[1, 1].Value = "ID";
                teamSheet.Cells[1, 2].Value = "Название";
                teamSheet.Cells[1, 3].Value = "Год основания";
                teamSheet.Cells[1, 4].Value = "Страна";
                teamSheet.Cells[1, 5].Value = "Призовой фонд ($)";

                // Форматирование заголовков
                using (var range = teamSheet.Cells[1, 1, 1, 5])
                {
                    range.Style.Font.Bold = true;
                    range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(255, 255, 224)); // LightYellow
                    range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    range.Style.Border.BorderAround(ExcelBorderStyle.Thin);
                }

                // Заполняем данными
                for (int i = 0; i < teams.Count; i++)
                {
                    var t = teams[i];
                    int row = i + 2;

                    teamSheet.Cells[row, 1].Value = t.ID_Teams;
                    teamSheet.Cells[row, 2].Value = t.Name;
                    teamSheet.Cells[row, 3].Value = t.Founded_year ?? 0;
                    teamSheet.Cells[row, 4].Value = t.Country ?? "";
                    teamSheet.Cells[row, 5].Value = t.Prize_pool;
                }

                teamSheet.Cells.AutoFitColumns();

                // ========================================
                // ЛИСТ 4: ИГРОКИ
                // ========================================
                var playerSheet = package.Workbook.Worksheets.Add("Игроки");

                // Заголовки
                playerSheet.Cells[1, 1].Value = "ID";
                playerSheet.Cells[1, 2].Value = "Никнейм";
                playerSheet.Cells[1, 3].Value = "Имя";
                playerSheet.Cells[1, 4].Value = "Фамилия";
                playerSheet.Cells[1, 5].Value = "Страна";
                playerSheet.Cells[1, 6].Value = "Призовой фонд ($)";
                playerSheet.Cells[1, 7].Value = "ID Команды";

                // Форматирование заголовков
                using (var range = playerSheet.Cells[1, 1, 1, 7])
                {
                    range.Style.Font.Bold = true;
                    range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(255, 182, 193)); // LightPink
                    range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    range.Style.Border.BorderAround(ExcelBorderStyle.Thin);
                }

                // Заполняем данными
                for (int i = 0; i < players.Count; i++)
                {
                    var p = players[i];
                    int row = i + 2;

                    playerSheet.Cells[row, 1].Value = p.ID_Players;
                    playerSheet.Cells[row, 2].Value = p.Nickname;
                    playerSheet.Cells[row, 3].Value = p.Name ?? "";
                    playerSheet.Cells[row, 4].Value = p.Surname ?? "";
                    playerSheet.Cells[row, 5].Value = p.Country ?? "";
                    playerSheet.Cells[row, 6].Value = p.Prize_pool;
                    playerSheet.Cells[row, 7].Value = p.ID_Teams?.ToString() ?? "";
                }

                playerSheet.Cells.AutoFitColumns();

                // Возвращаем байты
                return package.GetAsByteArray();
            }
        }
    }
}