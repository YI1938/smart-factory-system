using Microsoft.EntityFrameworkCore;
using SmartFactorySystem.Application.Interfaces;
using SmartFactorySystem.Application.Models;
using SmartFactorySystem.Infrastructure.Data;

namespace SmartFactorySystem.Infrastructure.Repositories;

public class MachineRepository : IMachineRepository
{
    private readonly FactoryDbContext _context;

    public MachineRepository(FactoryDbContext context)
    {
        _context = context;
    }

    public async Task<List<MachineStatus>> GetAllAsync()
    {
        return await _context.MachineStatuses
            .OrderBy(x => x.Id)
            .ToListAsync();
    }

    public async Task<MachineStatus?> GetByIdAsync(int id)
    {
        return await _context.MachineStatuses
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    // ① ステータスと停止理由を更新するロジック
    public async Task UpdateStatusAsync(int id, string status, string? stopReason = null)
    {
        var machine = await _context.MachineStatuses.FindAsync(id);
        if (machine == null) return;

        // 状態が今までと違うものに変わった場合のみ「状態変更時刻」を更新する
        if (machine.Status != status)
        {
            machine.LastStatusChangedAt = DateTime.UtcNow;
        }

        machine.Status = status;
        machine.StopReason = status == "Running" ? null : stopReason; // 稼働中は理由をクリア
        machine.LastUpdated = DateTime.UtcNow;

        await _context.SaveChangesAsync();
    }

    // ② 生産実績と不良品を「加算」するロジック（現場のPLC連携を想定）
    public async Task RecordProductionAsync(int id, int addedCount, int addedDefectCount)
    {
        var machine = await _context.MachineStatuses.FindAsync(id);
        if (machine == null) return;

        // 実績を加算
        machine.ActualProductionCount += addedCount;
        machine.DefectCount += addedDefectCount;
        machine.LastUpdated = DateTime.UtcNow;

        await _context.SaveChangesAsync();
    }
}