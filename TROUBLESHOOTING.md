# 🛠 Smart Factory System - トラブルシューティング & Tips集

本プロジェクトの開発・デプロイにおいて直面した技術的課題とその解決策の記録です。

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