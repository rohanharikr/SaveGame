using IGDB;
using SaveGame.Models;
using Microsoft.EntityFrameworkCore;

namespace SaveGame.Services
{
    internal class SQLiteService : DbContext
    {
        public DbSet<Game> Play { get; set; }
        public DbSet<Game> Playing { get; set; }
        public DbSet<Game> Played { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source = Data.db");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //ignore many-to-many relationship
            var propertyNames = typeof(Game)
                .GetProperties()
                .Where(p => 
                    p.PropertyType.IsGenericType && p.PropertyType.GetGenericTypeDefinition() == typeof(IdentitiesOrValues<>) ||
                    p.PropertyType.IsGenericType && p.PropertyType.GetGenericTypeDefinition() == typeof(IdentityOrValue<>)
                 )
                .Select(p => p.Name)
                .ToList();

            var entityTypes = modelBuilder.Model.GetEntityTypes()
                .Where(t => typeof(Game).IsAssignableFrom(t.ClrType));

            foreach (var entityType in entityTypes)
            {
                var entityTypeBuilder = modelBuilder.Entity(entityType.ClrType);
                foreach (var propertyName in propertyNames)
                    entityTypeBuilder.Ignore(propertyName);
            }
        }
    }
}
