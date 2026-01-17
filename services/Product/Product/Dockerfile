FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# --- ЗМІНА ПОЧИНАЄТЬСЯ ТУТ ---

# 1. Копіюємо .csproj файли ВСІХ проектів
# Ми повинні скопіювати файли проєктів-залежностей, щоб dotnet restore зміг їх знайти.
# Зверни увагу: шляхи "Product.Domain/..." повинні відповідати твоїм реальним назвам папок.

COPY ["Product/Product.API.csproj", "Product/"]
COPY ["Product.Domain/Product.Domain.csproj", "Product.Domain/"]
COPY ["Product.Infrastructure/Product.Infrastructure.csproj", "Product.Infrastructure/"]

# 2. Відновлюємо залежності
# Достатньо запустити restore для головного проєкту, він сам підтягне інші, 
# якщо ми правильно скопіювали .csproj файли вище.
RUN dotnet restore "Product/Product.API.csproj"

# --- ЗМІНА ЗАКІНЧУЄТЬСЯ ТУТ ---

# 3. Копіюємо решту файлів (код C#)
COPY . .

# 4. Білдимо
WORKDIR "/src/Product"
RUN dotnet build "Product.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Product.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Product.API.dll"]