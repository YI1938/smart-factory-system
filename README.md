# 🏭 SmartFactorySystem - 製造現場DXのための稼働監視プラットフォーム

## 📝 プロジェクト概要
本システムは、製造現場における「設備稼働の不透明さ」を解消し、**OEE（設備総合効率）の向上**を支援するためのダッシュボードシステムです。 
単なる状態表示に留まらず、停止理由の分析や生産進捗のリアルタイム可視化を三層アーキテクチャで実装しています。

---

## 🏗 設計思想：なぜ「三層アーキテクチャ」なのか
大手SIerでの大規模開発を想定し、「関心の分離」と「テスト容易性」を最大化するために採用しました。

### Presentation層 (Blazor Web App)
* **役割:** UI/UXの提供。
* **意図:** ロジックを持たせず、表示とユーザー入力の受け付けに特化。将来的にWebからモバイルアプリへ移行する際も、他の層を一切汚さずに差し替え可能です。

### Application層 (Business Logic)
* **役割:** 業務ルールの定義。
* **意図:** データベースや画面の仕様に依存しない「純粋なドメインロジック（異常検知時のログ出力、進捗率計算など）」を保持します。

### Infrastructure層 (Data Access)
* **役割:** データの永続化。
* **意図:** Entity Framework Coreを採用。現在はSQLiteですが、依存関係を抽象化しているため、設定一つでSQL ServerやAzure SQL Databaseへ切り替え可能な設計にしています。

---

## 💡 解決するビジネス課題
製造業のテックリードとしての視点から、以下の「現場の痛み」を解決する機能を実装しています。

* **「チョコ停」の可視化:** 単なる `Stopped` ではなく `StopReason` を必須化することで、現場の改善活動（PDCA）に必要なデータを提供。
* **OEE（設備総合効率）の基礎データ提供:** 計画数(Planned)と実績数(Actual)の対比、および良品率(QualityRate)をモデルレベルで保持し、損失コストの算出を容易にします。
* **初動対応の迅速化:** `LastStatusChangedAt` を保持することで、停止からの経過時間を可視化。長時間放置されている異常設備への迅速な対応を促します。

---

## 🛠 セットアップガイド（開発環境の構築）
PCがクリーンな状態からでも、以下の手順で動作確認が可能です。

### 1. 必要なツールのインストール
* **.NET 10.0 SDK:** 本システムのランタイム及びコンパイラ。
* **VS Code:** 推奨エディタ。
    * 拡張機能: `C# Dev Kit`, `SQLite Viewer` (DB確認用)

### 2. ローカルDBの構築
本プロジェクトはSQLiteを使用しているため、外部DBサーバーの構築は不要です。

```bash
# EF Core ツールをインストール（未導入の場合）
dotnet tool install --global dotnet-ef

# データベースを生成
dotnet ef database update --project Infrastructure --startup-project Presentation
'''

### 3. アプリケーションの実行
dotnet run --project Presentation/SmartFactorySystem.Presentation.csproj

### 4. 使用技術
 - Backend: C#, .NET 10.0
 - ORM: Entity Framework Core (SQLite)
 - Frontend: Blazor Server (Razor Components)
 - Architecture: Clean Architecture / Three-Layer
 - Architecture
 - Design: Bootstrap 5.0 + Bootstrap Icons