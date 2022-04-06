using Domain;
using Microsoft.EntityFrameworkCore;

namespace WebAPI.Persistence;

public class AppDbContext : DbContext
{ public DbSet<Actuator>? Actuators { get; set; }
    public DbSet<ClimateDevice>? ClimateDevices { get; set; }
    public DbSet<Measurement>? Measurements { get; set; }
    public DbSet<Room>? Rooms { get; set; }
    public DbSet<Sensor>? Sensors { get; set; }
    public DbSet<Settings>? Settings { get; set; }
    public DbSet<User>? Users { get; set; }


    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=Sep4Test;Trusted_Connection=True");
    }
}