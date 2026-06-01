using Microsoft.Extensions.Logging;
using SmartFactorySystem.Application.Interfaces;
using SmartFactorySystem.Application.Models;

namespace SmartFactorySystem.Application.Services;

public class MachineService
{
    private readonly IMachineRepository _repository;
    private readonly ILogger<MachineService> _logger;

    public MachineService(
        IMachineRepository repository,
        ILogger<MachineService> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<List<MachineStatus>> GetMachinesAsync()
    {
        return await _repository.GetAllAsync();
    }

    public async Task UpdateMachineStatusAsync(
        int machineId,
        string status)
    {
        await _repository.UpdateStatusAsync(machineId, status);

        if (status == "Error")
        {
            _logger.LogError(
                "設備異常発生 MachineId:{MachineId} 発生時刻:{Time}",
                machineId,
                DateTime.UtcNow);
        }
        else
        {
            _logger.LogInformation(
                "設備ステータス更新 MachineId:{MachineId} Status:{Status}",
                machineId,
                status);
        }
    }
}