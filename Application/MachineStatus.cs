namespace SmartFactorySystem.Application.Models;

public class MachineStatus
{
    public int Id { get; set; }

    public string MachineId { get; set; } = string.Empty;

    public string MachineName { get; set; } = string.Empty;

    /// <summary>
    /// Running / Stopped / Error
    /// </summary>
    public string Status { get; set; } = "Stopped";

    public DateTime LastUpdated { get; set; } = DateTime.UtcNow;
}