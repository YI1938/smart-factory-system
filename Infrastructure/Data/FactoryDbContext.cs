using Microsoft.EntityFrameworkCore;
using SmartFactorySystem.Application.Models;

namespace SmartFactorySystem.Infrastructure.Data;

public class FactoryDbContext : DbContext
{
    public FactoryDbContext(DbContextOptions<FactoryDbContext> options)
        : base(options)
    {
    }

    public DbSet<MachineStatus> MachineStatuses => Set<MachineStatus>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<MachineStatus>(entity =>
        {
            entity.HasKey(x => x.Id);

            entity.Property(x => x.MachineId).HasMaxLength(50).IsRequired();
            entity.Property(x => x.MachineName).HasMaxLength(100).IsRequired();
            entity.Property(x => x.Status).HasMaxLength(20).IsRequired();
            entity.Property(x => x.StopReason).HasMaxLength(200); // 停止理由を追加
        });

        // 顧客の「現場」をイメージした初期データ
        // 1つは順調、1つは遅延、1つは異常停止というストーリーを持たせています
        modelBuilder.Entity<MachineStatus>().HasData(
            new MachineStatus
            {
                Id = 1,
                MachineId = "LINE-A-PRSS",
                MachineName = "Aライン 高速プレス機",
                Status = "Running",
                PlannedProductionCount = 1000,
                ActualProductionCount = 450,
                DefectCount = 2,
                LastUpdated = new DateTime(2026, 6, 1, 0, 0, 0, DateTimeKind.Utc),
                LastStatusChangedAt = new DateTime(2026, 6, 1, 8, 0, 0, DateTimeKind.Utc)
            },
            new MachineStatus
            {
                Id = 2,
                MachineId = "LINE-B-WELD",
                MachineName = "Bライン 溶接ロボットアーム",
                Status = "PlannedStop",
                StopReason = "型替え作業中（段取り）",
                PlannedProductionCount = 500,
                ActualProductionCount = 120,
                DefectCount = 0,
                LastUpdated = new DateTime(2026, 6, 1, 0, 0, 0, DateTimeKind.Utc),
                LastStatusChangedAt = new DateTime(2026, 6, 1, 10, 30, 0, DateTimeKind.Utc)
            },
            new MachineStatus
            {
                Id = 3,
                MachineId = "LINE-C-PCKG",
                MachineName = "Cライン 自動梱包機",
                Status = "Error",
                StopReason = "搬送ベルト コンボイ詰まり",
                PlannedProductionCount = 2000,
                ActualProductionCount = 1800,
                DefectCount = 45,
                LastUpdated = new DateTime(2026, 6, 1, 0, 0, 0, DateTimeKind.Utc),
                LastStatusChangedAt = new DateTime(2026, 6, 1, 11, 15, 0, DateTimeKind.Utc)
            }
        );
    }
}