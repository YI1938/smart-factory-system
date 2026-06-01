using Microsoft.EntityFrameworkCore;
using SmartFactorySystem.Application.Interfaces;
using SmartFactorySystem.Application.Services;
using SmartFactorySystem.Infrastructure.Data;
using SmartFactorySystem.Infrastructure.Repositories;
using SmartFactorySystem.Presentation.Components; // 追加

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// SQLiteの接続設定
builder.Services.AddDbContext<FactoryDbContext>(options =>
{
    options.UseSqlite(builder.Configuration.GetConnectionString("FactoryDb"));
});

// 3層アーキテクチャのDI登録
builder.Services.AddScoped<IMachineRepository, MachineRepository>();
builder.Services.AddScoped<MachineService>();

var app = builder.Build();

// 初回起動時にDBを自動生成する処理
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<FactoryDbContext>();
    db.Database.Migrate();
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();