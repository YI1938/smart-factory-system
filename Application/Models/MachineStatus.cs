namespace SmartFactorySystem.Application.Models;

public class MachineStatus
{
    public int Id { get; set; }
    public string MachineId { get; set; } = string.Empty;
    public string MachineName { get; set; } = string.Empty;
    public string Status { get; set; } = "PlannedStop";
    public string? StopReason { get; set; }
    public int PlannedProductionCount { get; set; }
    public int ActualProductionCount { get; set; }
    public int DefectCount { get; set; }
    public DateTime LastUpdated { get; set; } = DateTime.UtcNow;
    public DateTime LastStatusChangedAt { get; set; } = DateTime.UtcNow;

    // --- ここから追加：ISA-95 レベル3（MES）拡張項目 ---

    /// <summary>
    /// 現在実行中の製造オーダー番号（レベル4/ERPからの指示を想定）
    /// </summary>
    public string CurrentWorkOrder { get; set; } = "WO-2026-0001";

    /// <summary>
    /// 稼働開始時刻（OEEの「稼働時間」算出用）
    /// </summary>
    public DateTime? LastStartedAt { get; set; }

    /// <summary>
    /// サイクルタイム（1個作るのにかかる標準秒数）
    /// </summary>
    public double StandardCycleTimeSeconds { get; set; } = 10.5;

    // --- ここまで追加 ---

    // ビジネスロジック（算出プロパティ）
    public double ProgressRate => PlannedProductionCount > 0 
        ? (double)ActualProductionCount / PlannedProductionCount * 100 
        : 0;

    public double QualityRate => ActualProductionCount > 0 
        ? (double)(ActualProductionCount - DefectCount) / ActualProductionCount * 100 
        : 100;

    // 追加：性能評価（理想に対してどれだけ作れたか）
    public double PerformanceRate => LastStartedAt.HasValue && StandardCycleTimeSeconds > 0
        ? (double)ActualProductionCount / ((DateTime.UtcNow - LastStartedAt.Value).TotalSeconds / StandardCycleTimeSeconds) * 100
        : 0;
}