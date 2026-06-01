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

    public double ProgressRate => PlannedProductionCount > 0 
        ? (double)ActualProductionCount / PlannedProductionCount * 100 
        : 0;

    public double QualityRate => ActualProductionCount > 0 
        ? (double)(ActualProductionCount - DefectCount) / ActualProductionCount * 100 
        : 100;
}