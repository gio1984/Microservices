using Microsoft.EntityFrameworkCore;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    public DbSet<ElevatorModel> DataElevator { get; set; }
    public DbSet<DoorsModel> DataDoor { get; set; }
    public DbSet<SmokeDetectorModel> DataSmokeDetector { get; set; }
}