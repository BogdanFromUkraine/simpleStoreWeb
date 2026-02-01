FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# --- ЗМІНА ПОЧИНАЄТЬСЯ ТУТ (адаптовано під Authorization) ---

# 1. Копіюємо .csproj файли ВСІХ проектів
# Шляхи повинні точно відповідати структурі твоєї папки рішення
# Головний проєкт → Authorization.API (він запускається)

COPY ["Authorization/Authorization.API.csproj", "Authorization/"]
COPY ["Authorization.Application/Authorization.Application.csproj", "Authorization.Application/"]
COPY ["Authorization.Domain/Authorization.Domain.csproj", "Authorization.Domain/"]
COPY ["Authorization.Infrastructure/Authorization.Infrastructure.csproj", "Authorization.Infrastructure/"]

# 2. Відновлюємо залежності
# Запускаємо restore саме для головного проєкту — він підтягне всі залежні
RUN dotnet restore "Authorization/Authorization.API.csproj"

# --- ЗМІНА ЗАКІНЧУЄТЬСЯ ТУТ ---

# 3. Копіюємо решту файлів (весь код C#)
COPY . .

# 4. Білдимо
WORKDIR "/src/Authorization"
RUN dotnet build "Authorization.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Authorization.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Authorization.API.dll"]