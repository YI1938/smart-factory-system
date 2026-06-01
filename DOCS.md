## 概要

このリポジトリは、シンプルな生産ライン監視（Production Monitoring）サンプルです。3 層アーキテクチャ（Application / Infrastructure / Presentation）で構成され、設備の稼働状態、停止理由、実績を管理します。

## プロジェクト構成（主要ファイル）

- [SmartFactorySystem.slnx](SmartFactorySystem.slnx)
- Application/
    - [Class1.cs](Application/Class1.cs)
    - [IMachineRepository.cs](Application/IMachineRepository.cs)
    - [MachineService.cs](Application/MachineService.cs)
    - [MachineStatus.cs](Application/MachineStatus.cs)
    - [SmartFactorySystem.Application.csproj](Application/SmartFactorySystem.Application.csproj)

- Infrastructure/
    - [Class1.cs](Infrastructure/Class1.cs)
    - [FactoryDbContext.cs](Infrastructure/FactoryDbContext.cs)
    - [MachineRepository.cs](Infrastructure/MachineRepository.cs)
    - [Migrations/](Infrastructure/Migrations/)
    - [SmartFactorySystem.Infrastructure.csproj](Infrastructure/SmartFactorySystem.Infrastructure.csproj)

- Presentation/
    - [appsettings.json](Presentation/appsettings.json)
    - [appsettings.Development.json](Presentation/appsettings.Development.json)
    - [Program.cs](Presentation/Program.cs)
    - Components/
        - Layout: [MainLayout.razor](Presentation/Components/Layout/MainLayout.razor), [NavMenu.razor](Presentation/Components/Layout/NavMenu.razor)
        - Pages: [MachineList.razor](Presentation/Components/Pages/MachineList.razor), [Home.razor](Presentation/Components/Pages/Home.razor)
    - [SmartFactorySystem.Presentation.csproj](Presentation/SmartFactorySystem.Presentation.csproj)

## 各ファイルの役割と動作（要点）

- ソリューション
    - `SmartFactorySystem.slnx`: 3 プロジェクトを束ねる Visual Studio / dotnet ソリューション。

- Application
    - `Class1.cs` ([Application/Class1.cs](Application/Class1.cs)): テンプレートの空クラス（現状未使用のサンプル）。
    - `IMachineRepository.cs` ([Application/IMachineRepository.cs](Application/IMachineRepository.cs)):
        - データアクセスの抽象インターフェース。
        - `GetAllAsync()`, `GetByIdAsync(id)`, `UpdateStatusAsync(id, status, stopReason?)`, `RecordProductionAsync(id, addedCount, addedDefectCount)` を定義。
    - `MachineService.cs` ([Application/MachineService.cs](Application/MachineService.cs)):
        - ビジネスロジック層。`IMachineRepository` を注入してデータ取得・更新を行う。
        - `GetMachinesAsync()` で一覧を取得、`UpdateMachineStatusAsync(...)` でステータス更新（エラー時にログ出力）、`RecordProductionAsync(...)` で実績加算とログ出力を行う。
    - `MachineStatus` モデル（ソースはプロジェクト内にクラスファイルとして見つからない場合がありますが、スキーマはマイグレーションに定義されています。主なプロパティは下記）：
        - Id, MachineId, MachineName, Status, StopReason, PlannedProductionCount, ActualProductionCount, DefectCount, LastUpdated, LastStatusChangedAt
        - スキーマ詳細は [Infrastructure/Migrations/FactoryDbContextModelSnapshot.cs](Infrastructure/Migrations/FactoryDbContextModelSnapshot.cs) を参照してください。

- Infrastructure
    - `FactoryDbContext.cs` ([Infrastructure/FactoryDbContext.cs](Infrastructure/FactoryDbContext.cs)):
        - EF Core の `DbContext`。
        - `DbSet<MachineStatus> MachineStatuses` を公開し、エンティティの構成（最大長や必須項目、`StopReason` の追加など）と初期データ（サンプル3件）を定義する。
    - `MachineRepository.cs` ([Infrastructure/MachineRepository.cs](Infrastructure/MachineRepository.cs)):
        - `IMachineRepository` の具象実装。
        - `GetAllAsync()` で全件取得、`GetByIdAsync()` で単一取得、`UpdateStatusAsync()` で状態／停止理由／更新時刻を更新、`RecordProductionAsync()` で実績・不良を加算する。
    - `Migrations/`:
        - EF Core のマイグレーションファイル群。スナップショットにはサンプルデータとスキーマが含まれます。

- Presentation (Blazor)
    - `Program.cs` ([Presentation/Program.cs](Presentation/Program.cs)):
        - アプリ起動処理、DI 登録、SQLite 接続設定（接続文字列キー `FactoryDb`）、自動マイグレーション適用（起動時に `db.Database.Migrate()` を呼ぶ）、Razor Components/Blazor の設定を行う。
        - `IMachineRepository` → `MachineRepository`、`MachineService` をスコープ登録しているため、UI で注入して利用できる。
    - `MachineList.razor` ([Presentation/Components/Pages/MachineList.razor](Presentation/Components/Pages/MachineList.razor)):
        - 生産ラインの一覧表示ページ。
        - `MachineService` を注入し、`GetMachinesAsync()` でロード、ボタンで `UpdateMachineStatusAsync()` を呼び出して状態更新を行い、再ロードする。
    - レイアウト・共通コンポーネント（ナビ・Reconnect モーダルなど）は UI とリアルタイム再接続挙動を補助します。

## ビルドと実行手順

1. リポジトリルートで復元とビルド:

```bash
dotnet restore
dotnet build
```

2. Presentation（Blazor）を実行:

```bash
dotnet run --project Presentation/SmartFactorySystem.Presentation.csproj
```

3. データベース初期化 / マイグレーション適用（必要時）:

```bash
dotnet ef database update --project Infrastructure --startup-project Presentation
```
