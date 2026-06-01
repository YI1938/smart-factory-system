using Microsoft.EntityFrameworkCore;
using SmartFactorySystem.Application.Interfaces;
using SmartFactorySystem.Application.Models;
using SmartFactorySystem.Infrastructure.Data;

namespace SmartFactorySystem.Infrastructure.Repositories;

public class MachineRepository : IMachineRepository
{
    private readonly FactoryDbContext _context;

    public MachineRepository(
        FactoryDbContext context)
    {
        _context = context;
    }

    public async Task<List<MachineStatus>> GetAllAsync()
    {
        return await _context.MachineStatuses
            .OrderBy(x => x.MachineId)
            .ToListAsync();
    }

    public async Task<MachineStatus?> GetByIdAsync(int id)
    {
        return await _context.MachineStatuses
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task UpdateStatusAsync(
        int id,
        string status)
    {
        var machine = await _context.MachineStatuses
            .FirstOrDefaultAsync(x => x.Id == id);

        if (machine == null)
            return;

        machine.Status = status;
        machine.LastUpdated = DateTime.UtcNow;

        await _context.SaveChangesAsync();
    }
}