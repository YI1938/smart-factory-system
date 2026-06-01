using Microsoft.EntityFrameworkCore;
using SmartFactorySystem.Application.Models;

namespace SmartFactorySystem.Infrastructure.Data;

public class FactoryDbContext : DbContext
{
    public FactoryDbContext(
        DbContextOptions<FactoryDbContext> options)
        : base(options)
    {
    }

    public DbSet<MachineStatus> MachineStatuses => Set<MachineStatus>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<MachineStatus>(entity =>
        {
            entity.HasKey(x => x.Id);

            entity.Property(x => x.MachineId)
                .HasMaxLength(50)
                .IsRequired();

            entity.Property(x => x.MachineName)
                .HasMaxLength(100)
                .IsRequired();

            entity.Property(x => x.Status)
                .HasMaxLength(20)
                .IsRequired();
        });

        modelBuilder.Entity<MachineStatus>().HasData(
            new MachineStatus
            {
                Id = 1,
                MachineId = "MC-001",
                MachineName = "プレス機1号",
                Status = "Running",
                LastUpdated = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            },
            new MachineStatus
            {
                Id = 2,
                MachineId = "MC-002",
                MachineName = "溶接機1号",
                Status = "Stopped",
                LastUpdated = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            }
        );
    }
}