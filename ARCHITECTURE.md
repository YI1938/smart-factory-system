# 🏛 Azure Architecture & Deployment Registry

本プロジェクトにおいて、GitHubのソースコード以外で実施したインフラ構築、セキュリティ設計、およびクラウドのリソース構成についての記録です。

---

## 1. サーバーレス・コンテナ基盤の構築
当初は App Service を検討しましたが、コスト効率とスケーラビリティを考慮し、**Azure Container Apps (ACA)** を採用しました。
- **Azure Container Registry (ACR):** - Dockerイメージのプライベート管理。
    - GitHub Actions等との親和性を考慮したタグ管理（v1〜v4）の実施。
- **Azure Container Apps:**
    - サーバーレス環境でのコンテナ実行。
    - 環境変数を用いた接続文字列の分離管理。

## 2. セキュリティ設計 (Network & Access)
「工場内のデータを扱う」という想定に基づき、公開設定に以下の制限を実装しました。
- **Ingress Access Restrictions:**
    - 特定のIPアドレス（管理者環境、開発環境、現場のモバイルデバイス等）のみを許可するホワイトリスト方式をCLIから実装。
- **セキュアなエンドポイント:**
    - HTTPSの強制と、不要なポートの閉塞。

## 3. インフラ・監視の統合 (Observability)
アプリが「動いているか」だけでなく「どう動いているか」を可視化するため、**Application Insights** を中核とした監視基盤を構築しました。
- **Log Analytics Workspace:** - 構造化されたログの長期保存とKQLを用いた高度な分析環境の整備。
- **リソース間の垂直統合:**
    - ISA-95モデルに基づき、現場イベントが即座にクラウドの監視ログとして反映されるようテレメトリ送信を構成。

## 4. 使用した主要ツール・技術
- **Infrastructure as Code (CLI):** Azure CLI を多用し、環境構築の再現性を確保。
- **Containerization:** Docker Desktop を使用したマルチプラットフォーム（linux/amd64）ビルド。