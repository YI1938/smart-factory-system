## 概要

このリポジトリは「生産ライン監視」のサンプル実装です。3 層アーキテクチャ（Application / Infrastructure / Presentation）で設計され、設備の稼働状態管理、停止理由の保持、実績（良品 / 不良品）の記録・表示を行います。

## リポジトリ構成（主要ファイル）

- [SmartFactorySystem.slnx](SmartFactorySystem.slnx)
- Application/
    - [Models/MachineStatus.cs](Application/Models/MachineStatus.cs)
    - [Interfaces/IMachineRepository.cs](Application/Interfaces/IMachineRepository.cs)
    - [Services/MachineService.cs](Application/Services/MachineService.cs)
    - [SmartFactorySystem.Application.csproj](Application/SmartFactorySystem.Application.csproj)
- Infrastructure/
    - [Data/FactoryDbContext.cs](Infrastructure/Data/FactoryDbContext.cs)
    - [Repositories/MachineRepository.cs](Infrastructure/Repositories/MachineRepository.cs)
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

## 各フォルダと主要ファイルの説明

### Application
- `Application/Models/MachineStatus.cs`:
    - アプリケーションのドメインモデル（POCO）。主要プロパティ: `Id`, `MachineId`, `MachineName`, `Status`, `StopReason`, `PlannedProductionCount`, `ActualProductionCount`, `DefectCount`, `LastUpdated`, `LastStatusChangedAt`。
    - 追加の MES 拡張プロパティ: `CurrentWorkOrder`, `LastStartedAt`, `StandardCycleTimeSeconds`。
    - 計算プロパティ: `ProgressRate`, `QualityRate`, `PerformanceRate` を備え、UI 表示用の算出値を提供します。

- `Application/Interfaces/IMachineRepository.cs`:
    - データアクセスの抽象インターフェース。定義メソッド:
        - `GetAllAsync()` — 全設備一覧取得
        - `GetByIdAsync(id)` — 単一設備取得
        - `UpdateStatusAsync(id, status, stopReason?)` — ステータス（と停止理由）の更新
        - `RecordProductionAsync(id, addedCount, addedDefectCount)` — 実績／不良の加算

- `Application/Services/MachineService.cs`:
    - ビジネスロジック層。`IMachineRepository` と `ILogger<MachineService>` を注入して利用します。
    - 主な公開メソッド:
        - `GetMachinesAsync()` — 一覧取得
        - `UpdateMachineStatusAsync(id, status, stopReason?)` — リポジトリ呼出しでステータス更新（ログは Presentation 層またはサービス側で行う方針に合わせて拡張可能）
        - `RecordProductionAsync(id, addedCount, addedDefectCount)` — 実績の記録

### Infrastructure
- `Infrastructure/Data/FactoryDbContext.cs`:
    - EF Core の `DbContext`。`DbSet<MachineStatus> MachineStatuses` を公開。
    - `OnModelCreating` でカラム制約（長さ、必須）とシードデータ（3 件のサンプル）を定義。

- `Infrastructure/Repositories/MachineRepository.cs`:
    - `IMachineRepository` の具象実装。DB 操作は `FactoryDbContext` 経由で行う。
    - 主なロジック:
        - `GetAllAsync()` — 順番付きで一覧取得
        - `GetByIdAsync(id)` — 単一取得
        - `UpdateStatusAsync(id, status, stopReason?)` — 状態が変わった場合は `LastStatusChangedAt` を更新。`Running` の場合は `StopReason` をクリアし、`LastUpdated` を更新して保存。
        - `RecordProductionAsync(id, addedCount, addedDefectCount)` — `ActualProductionCount` と `DefectCount` を加算し `LastUpdated` を更新して保存。

- `Infrastructure/Migrations/`:
    - EF Core マイグレーションおよびスナップショットを格納。最新のスナップショット（`FactoryDbContextModelSnapshot.cs`）により、データベースの最終スキーマ（MES 拡張フィールドを含む）が確認できます。

### Presentation (Blazor)
- `Presentation/Program.cs`:
    - アプリケーション起動処理と DI 登録を行うエントリポイント。
    - SQLite 接続は `appsettings.json` の `ConnectionStrings:FactoryDb` を使用（デフォルト: `Data Source=factory.db`）
    - サービス登録例: `IMachineRepository` → `MachineRepository`, `MachineService` をスコープ登録。
    - 起動時に `db.Database.Migrate()` を呼び、マイグレーションを自動適用する実装が含まれます（開発環境では便利ですが、本番では運用方針に合わせて調整してください）。

- UI コンポーネント:
    - `Presentation/Components/Pages/MachineList.razor` — メインの稼働モニター画面。`MachineService` を注入し、一覧取得・状態更新（理由付き）・進捗・品質表示を行います。
    - レイアウト / 共通コンポーネントは `Presentation/Components/Layout/` に配置され、ナビゲーションや再接続モーダル等を提供します。

### 動作の流れ（代表例）

1. ブラウザで `MachineList` ページを開く。
2. ページが `MachineService.GetMachinesAsync()` を呼び、`IMachineRepository.GetAllAsync()` 経由で DB から `MachineStatus` 一覧を取得する。
3. ユーザーがボタン操作で状態を変更すると、`MachineService.UpdateMachineStatusAsync(...)` が呼ばれ、`MachineRepository.UpdateStatusAsync(...)` が DB を更新する。
4. センサー／外部システムと連携する想定では、`RecordProductionAsync(...)` を使って実績を逐次加算します。

### データベーススキーマ（要約、スナップショット準拠）

- テーブル: `MachineStatuses`
    - `Id` (INTEGER PK)
    - `MachineId` (TEXT, max 50, required)
    - `MachineName` (TEXT, max 100, required)
    - `Status` (TEXT, max 20, required)
    - `StopReason` (TEXT, max 200, optional)
    - `PlannedProductionCount` (INTEGER)
    - `ActualProductionCount` (INTEGER)
    - `DefectCount` (INTEGER)
    - `LastUpdated` (TEXT / DateTime)
    - `LastStatusChangedAt` (TEXT / DateTime)
    - MES 拡張: `CurrentWorkOrder` (TEXT), `LastStartedAt` (TEXT / nullable), `StandardCycleTimeSeconds` (REAL)

