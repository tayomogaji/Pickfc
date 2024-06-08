using Microsoft.EntityFrameworkCore;
using Pickfc.Model.Entities;

namespace Pickfc.Model.Context
{
    public class PickfcContext : DbContext
    {
        public string ConnectionString { get; }

        public PickfcContext(string connectionString) : base()
        { 
            ConnectionString = connectionString;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) 
        {
            optionsBuilder.UseSqlServer(ConnectionString);
            base.OnConfiguring(optionsBuilder);
        }

        DbSet<Art> Arts { get; set; }
        DbSet<Backup> Backups { get; set; }
        DbSet<User> Users { get; set; }
        DbSet<Comp> Comps { get; set; }
        DbSet<Game> Games { get; set; }
        DbSet<CompTeam> CompTeams { get; set; }
        DbSet<Team> Teams { get; set;}
        DbSet<Player> Players { get; set; }
        DbSet<Round> Rounds { get; set; }
        DbSet<Fixture> Fixtures { get; set; }
        DbSet<Pick> Picks { get; set; }

    }
}
