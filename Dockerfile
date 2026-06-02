# 1. ビルド環境の指定 (.NET 10 SDK)
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# 2. プロジェクトファイルのコピーと復元 (キャッシュ効率化のため)
COPY ["Presentation/SmartFactorySystem.Presentation.csproj", "Presentation/"]
COPY ["Application/SmartFactorySystem.Application.csproj", "Application/"]
COPY ["Infrastructure/SmartFactorySystem.Infrastructure.csproj", "Infrastructure/"]
RUN dotnet restore "Presentation/SmartFactorySystem.Presentation.csproj"

# 3. ソースコード全体のコピーとビルド
COPY . .
WORKDIR "/src/Presentation"
RUN dotnet build "SmartFactorySystem.Presentation.csproj" -c Release -o /app/build

# 4. 公開用ファイルの作成
FROM build AS publish
RUN dotnet publish "SmartFactorySystem.Presentation.csproj" -c Release -o /app/publish /p:UseAppHost=false

# 5. 実行用環境の作成 (軽量なASP.NETランタイム)
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .

# ポート番号の設定
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

ENTRYPOINT ["dotnet", "SmartFactorySystem.Presentation.dll"]