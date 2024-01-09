using IGDB.Models;
using Microsoft.EntityFrameworkCore;

namespace SaveGame.Services
{
    internal class SQLiteService : DbContext
    {
        public DbSet<Game> Games { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source = Data.db");
        }
    }
}
