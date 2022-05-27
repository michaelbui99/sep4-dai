using System;
using Domain;
using Microsoft.EntityFrameworkCore;

namespace WebAPI.Persistence
{
    public class AppDbContext : DbContext
    {
        public DbSet<Room>? Rooms { get; set; }
        public DbSet<Measurement>? Measurements { get; set; }
        public DbSet<ClimateDevice>? Devices { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(ConnectionStringGenerator.GetConnectionStringFromEnvironment());
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Room>().HasAlternateKey(room => room.RoomName);
        }
    }
}