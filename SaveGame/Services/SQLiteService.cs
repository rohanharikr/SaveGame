using IGDB;
using IGDB.Models;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;
using System.Reflection.Metadata;

namespace SaveGame.Services
{
    internal class SQLiteService : DbContext
    {
        public DbSet<Game> Games { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source = Data.db");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //ignore mappings / not being used
            modelBuilder.Entity<Game>()
                .Ignore(p => p.Collection)
                .Ignore(p => p.Collections)
                .Ignore(p => p.ParentGame)
                .Ignore(p => p.VersionParent)
                .Ignore(p => p.Franchise);
            modelBuilder.Entity<Company>()
                .Ignore(p => p.ChangedCompanyId)
                .Ignore(p => p.Parent);

            modelBuilder.Entity<Cover>()
                .HasOne(g => g.GameLocalization)
                .WithOne(c => c.Cover)
                .HasForeignKey<Cover>(c => c.Id);
            modelBuilder.Entity<Game>()
                .HasOne(g => g.Cover)
                .WithOne(c => c.Game)
                .HasForeignKey<Cover>(c => c.Id);

            base.OnModelCreating(modelBuilder);
        }
    }
}
