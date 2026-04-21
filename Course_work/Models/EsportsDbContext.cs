using Microsoft.EntityFrameworkCore;
using Course_Work.Models;

namespace Course_Work.Models
{
    public class EsportsDbContext : DbContext
    {
        public EsportsDbContext(DbContextOptions<EsportsDbContext> options) : base(options) { }

        // Основные таблицы
        public DbSet<Game> Games { get; set; }
        public DbSet<News> News { get; set; }
        public DbSet<Tournament> Tournaments { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<Player> Players { get; set; }
        public DbSet<Match> Matches { get; set; }
        public DbSet<User> Users { get; set; }

        // Промежуточные таблицы 
        public DbSet<GamesNews> GamesNews { get; set; }
        public DbSet<PlayersGames> PlayersGames { get; set; }
        public DbSet<TeamsTournament> TeamsTournaments { get; set; }
        public DbSet<TeamsMatch> TeamsMatches { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Games_News
            modelBuilder.Entity<GamesNews>()
                .HasKey(gn => new { gn.ID_Games, gn.ID_News });

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

            // Players_Games
            modelBuilder.Entity<PlayersGames>()
                .HasKey(pg => new { pg.ID_Players, pg.ID_Games });

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

            // Teams_Tournament
            modelBuilder.Entity<TeamsTournament>()
                .HasKey(tt => new { tt.ID_Teams, tt.ID_Tournament });

            modelBuilder.Entity<TeamsTournament>()
                .HasOne(tt => tt.Team)
                .WithMany(t => t.TeamsTournaments)
                .HasForeignKey(tt => tt.ID_Teams)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<TeamsTournament>()
                .HasOne(tt => tt.Tournament)
                .WithMany(t => t.TeamsTournaments)
                .HasForeignKey(tt => tt.ID_Tournament)
                .OnDelete(DeleteBehavior.Cascade);

            // Teams_Matches
            modelBuilder.Entity<TeamsMatch>()
                .HasKey(tm => new { tm.ID_Teams, tm.ID_Matches });

            modelBuilder.Entity<TeamsMatch>()
                .HasOne(tm => tm.Team)
                .WithMany(t => t.TeamsMatches)
                .HasForeignKey(tm => tm.ID_Teams)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<TeamsMatch>()
                .HasOne(tm => tm.Match)
                .WithMany(m => m.TeamsMatches)
                .HasForeignKey(tm => tm.ID_Matches)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<User>()
                .HasIndex(u => u.Login)
                .IsUnique();
        }
    }
}