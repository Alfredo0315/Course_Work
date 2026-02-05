using Microsoft.EntityFrameworkCore;

namespace Course_Work.Models
{
    /// <summary>
    /// Контекст базы данных для приложения киберспортивных соревнований
    /// </summary>
    public class EsportsDbContext : DbContext
    {
        public EsportsDbContext(DbContextOptions<EsportsDbContext> options)
            : base(options)
        {
        }

        // DbSet'ы для всех таблиц
        public DbSet<Player> Players { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<Tournament> Tournament { get; set; }
        public DbSet<Match> Matches { get; set; }
        public DbSet<Game> Games { get; set; }
        public DbSet<News> News { get; set; }
        public DbSet<PlayersGames> Players_Games { get; set; }
        public DbSet<GamesNews> Games_News { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Настройка связей и индексов
            
            // Player - Team (многие к одному)
            modelBuilder.Entity<Player>()
                .HasOne(p => p.Team)
                .WithMany(t => t.Players)
                .HasForeignKey(p => p.ID_Teams)
                .OnDelete(DeleteBehavior.SetNull);

            // Team - Tournament (многие к одному)
            modelBuilder.Entity<Team>()
                .HasOne(t => t.Tournament)
                .WithMany(tour => tour.Teams)
                .HasForeignKey(t => t.ID_Tournament)
                .OnDelete(DeleteBehavior.SetNull);

            // Match - Tournament (многие к одному)
            modelBuilder.Entity<Match>()
                .HasOne(m => m.Tournament)
                .WithMany(t => t.Matches)
                .HasForeignKey(m => m.ID_Tournament)
                .OnDelete(DeleteBehavior.Cascade);

            // Players_Games (многие ко многим)
            modelBuilder.Entity<PlayersGames>()
                .HasOne(pg => pg.Player)
                .WithMany(p => p.PlayersGames)
                .HasForeignKey(pg => pg.ID_Players)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<PlayersGames>()
                .HasOne(pg => pg.Game)
                .WithMany(g => g.PlayersGames)
                .HasForeignKey(pg => pg.ID_Games)
                .OnDelete(DeleteBehavior.Cascade);

            // Games_News (многие ко многим)
            modelBuilder.Entity<GamesNews>()
                .HasOne(gn => gn.Game)
                .WithMany(g => g.GamesNews)
                .HasForeignKey(gn => gn.ID_Games)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<GamesNews>()
                .HasOne(gn => gn.News)
                .WithMany(n => n.GamesNews)
                .HasForeignKey(gn => gn.ID_News)
                .OnDelete(DeleteBehavior.Cascade);

            // Настройка индексов для оптимизации запросов
            modelBuilder.Entity<Player>()
                .HasIndex(p => p.Nickname);

            modelBuilder.Entity<Player>()
                .HasIndex(p => p.Country);

            modelBuilder.Entity<Team>()
                .HasIndex(t => t.Name);

            modelBuilder.Entity<Tournament>()
                .HasIndex(t => t.Start_date);
        }
    }
}