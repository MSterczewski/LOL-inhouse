using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection.Metadata;
using DatabaseModels;
using Microsoft.EntityFrameworkCore;

namespace Database
{
    public class MyDbContext : DbContext
    {
        public DbSet<Player> Players { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //optionsBuilder.UseSqlite($"Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=YourDatabase");
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlServer(
                @"Server=(localdb)\mssqllocaldb;Database=Inhouse;Trusted_Connection=True"
            );
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            SetupPlayer(modelBuilder);
        }

        private void SetupPlayer(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Player>().HasKey(p => p.Id);
            modelBuilder.Entity<Player>().HasAlternateKey(p => p.Nickname);
            modelBuilder.Entity<Player>().Property(p => p.Id).UseIdentityColumn();
        }
    }
}
