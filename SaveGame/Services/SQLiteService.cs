using IGDB;
using IGDB.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace SaveGame.Services
{
    internal class SQLiteService : DbContext
    {

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source = Data.db");
        }

        public DbSet<Game> Play { get; set; }
        public DbSet<Game> Playing { get; set; }
        public DbSet<Game> Played { get; set; }
    }
}
