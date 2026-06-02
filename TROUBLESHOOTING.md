# 🛠 Smart Factory System - トラブルシューティング & Tips集

本プロジェクトの開発・デプロイにおいて直面した技術的課題とその解決策の記録です。

---

## 🚀 新機能：Application Insights による運用監視の導入 (feat)

### 目的・背景
ISA-95の垂直統合モデルに基づき、現場（Level 2 / Sensing）で発生したエラーや設備のインシデント情報を、クラウド上の上位層（Level 4）へリアルタイムに転送・可視化し、一元管理できる仕組みを検証するため。

### 実装内容
- **パッケージ導入:** `Microsoft.ApplicationInsights.AspNetCore` をプロジェクトへ追加。
- **テレメトリ送信の実装:** 監視稼働中の保全画面（`Maintenance.razor`）が読み込まれた際、`TelemetryClient` を用いてカスタムイベント（`MachineMaintenanceRequired`）を発行。
- **メタデータの付与:** 収集データに `MachineId`、`ErrorType`、`Location`、`ISA95_Level` などのカスタムディメンションを付与し、Azureポータルのログ（Log Analytics / KQLクエリ）から正確な絞り込み分析を行える構成を構築。

---

## 1. Azure CLIのコマンド非推奨・バージョン差異
### 🚨 事象
Azure Container AppsへのIP制限追加時、ドキュメントにあった `az containerapp ingress access-restriction add` や `show` コマンドが認識されずエラーとなった。

### 💡 原因 & 対策
CLI拡張機能のアップデートによるサブコマンドの変更。最新仕様に合わせ、以下の通り統合コマンドや上位コマンドで代替した。
* **対策1:** `add` ではなく `set` コマンドを使用してルールを反映。
* **対策2:** `ingress access-restriction show` が通らない環境のため、`az containerapp ingress show` でネットワーク設定全体を出力し、JSON内の `ipSecurityRestrictions` を確認。

---

## 2. ASP.NET Core (.NET 10) における DI（依存性の注入）エラー
### 🚨 事象
Application Insightsの導入後、デプロイした環境で `An error occurred while processing your request.`（Development Modeエラー）が発生し、画面がクラッシュした。

### 💡 原因 & 対策
`Program.cs` 内でのコードの記述順序ミス（`builder` の生成より前、および二重にサービス登録が走っていたこと）によるDIエラー。また、最新の.NET仕様に伴う接続文字列の渡し方の不整合。
* **対策:** `WebApplication.CreateBuilder(args)` を最上部に配置し、最新の型安全な記法（`options.ConnectionString = ...`）に修正。デプロイ前にローカル環境で `dotnet build` を実行して健全性を担保する運用に切り替えた。