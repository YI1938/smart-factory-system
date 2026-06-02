using Microsoft.EntityFrameworkCore;
using SmartFactorySystem.Application.Interfaces;
using SmartFactorySystem.Application.Services;
using SmartFactorySystem.Infrastructure.Data;
using SmartFactorySystem.Infrastructure.Repositories;
using SmartFactorySystem.Presentation.Components;

// 1. 【最重要】まずは必ず builder を作成する
var builder = WebApplication.CreateBuilder(args);

// 2. Application Insights の設定（最新の記法）
var appInsightsConnectionString = "InstrumentationKey=459b7dd6-b941-4e83-9e69-3eecdcfa9c4e;IngestionEndpoint=https://japaneast-1.in.applicationinsights.azure.com/;LiveEndpoint=https://japaneast.livediagnostics.monitor.azure.com/;ApplicationId=a9576ac3-92ed-4653-b10b-2ae433814662";
builder.Services.AddApplicationInsightsTelemetry(options => 
{
    options.ConnectionString = appInsightsConnectionString;
});

// 3. 既存のサービス登録
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

// 4. アプリケーションのビルド（これより上で builder をいじる）
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