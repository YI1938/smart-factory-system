namespace SmartFactorySystem.Application.Interfaces;

using SmartFactorySystem.Application.Models;

public interface IMachineRepository
{
    Task<List<MachineStatus>> GetAllAsync();

    Task<MachineStatus?> GetByIdAsync(int id);

    // 変更：停止理由(stopReason)を受け取れるように拡張
    Task UpdateStatusAsync(int id, string status, string? stopReason = null);

    // 追加：生産実績（良品・不良品）を加算するためのメソッド
    Task RecordProductionAsync(int id, int addedCount, int addedDefectCount);
}