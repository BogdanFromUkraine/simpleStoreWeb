FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# --- КОПІЮВАННЯ ПРОЄКТІВ (ШАРІВ) ---

# 1. Копіюємо .csproj файли для всіх шарів Onion Architecture
# Важливо скопіювати їх усі, щоб dotnet restore зміг побудувати дерево залежностей.

# Головний API
COPY ["CartService/CartService.API.csproj", "CartService/"]

# Шар бізнес-логіки
COPY ["CartService.Application/CartService.Application.csproj", "CartService.Application/"]

# Шар інфраструктури (тут Kafka, БД)
COPY ["CartService.Infrastructure/CartService.Infrastructure.csproj", "CartService.Infrastructure/"]

# Шар домену (якщо він є, краще додати, щоб не було помилок)
COPY ["CartService.Domain/CartService.Domain.csproj", "CartService.Domain/"]

# 2. Відновлюємо залежності
RUN dotnet restore "CartService/CartService.API.csproj"

# --- КІНЕЦЬ КОПІЮВАННЯ ЗАЛЕЖНОСТЕЙ ---

# 3. Копіюємо весь інший код
COPY . .

# 4. Білдимо проєкт
WORKDIR "/src/CartService"
RUN dotnet build "CartService.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

# 5. Публікуємо (створюємо фінальні файли для запуску)
FROM build AS publish
ARG BUILD_CONFIGURATION=Debug
RUN dotnet publish "CartService.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# 6. Фінальний образ
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CartService.API.dll"]