ProductionMonitoring
│
├─ Application
│   ├─ Models
│   │   └─ MachineStatus.cs
│   ├─ Interfaces
│   │   └─ IMachineRepository.cs
│   └─ Services
│       └─ MachineService.cs
│
├─ Infrastructure
│   ├─ Data
│   │   └─ FactoryDbContext.cs
│   └─ Repositories
│       └─ MachineRepository.cs
│
└─ Presentation (Blazor Server または Blazor Web App)
    ├─ Components
    │   └─ Pages
    │       └─ MachineList.razor
    └─ Program.cs