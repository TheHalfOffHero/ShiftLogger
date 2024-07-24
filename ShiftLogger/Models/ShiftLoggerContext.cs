using Microsoft.EntityFrameworkCore;

namespace ShiftLogger.Models;

public class ShiftLoggerContext : DbContext
{
    public DbSet<Shift> Shifts { get; set; }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql("Host=localhost;Database=ShiftLoggerDb;Username=postgres;Password=postgres");
    }
}