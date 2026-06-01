using Microsoft.Extensions.Logging;
using SmartFactorySystem.Application.Interfaces;
using SmartFactorySystem.Application.Models;

namespace SmartFactorySystem.Application.Services; // ←ここをServicesにする

public class MachineService
{
    private readonly IMachineRepository _repository;
    private readonly ILogger<MachineService> _logger;

    public MachineService(IMachineRepository repository, ILogger<MachineService> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<List<MachineStatus>> GetMachinesAsync()
    {
        return await _repository.GetAllAsync();
    }

    public async Task UpdateMachineStatusAsync(int machineId, string status, string? stopReason = null)
    {
        await _repository.UpdateStatusAsync(machineId, status, stopReason);
        // ...以下、先ほど送ってくれたログ出力ロジックをそのまま入れる
    }

    public async Task RecordProductionAsync(int machineId, int addedCount, int addedDefectCount)
    {
        await _repository.RecordProductionAsync(machineId, addedCount, addedDefectCount);
    }
}