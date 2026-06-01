using SmartFactorySystem.Application.Models;

namespace SmartFactorySystem.Application.Interfaces;

public interface IMachineRepository
{
    Task<List<MachineStatus>> GetAllAsync();

    Task<MachineStatus?> GetByIdAsync(int id);

    Task UpdateStatusAsync(int id, string status);
}