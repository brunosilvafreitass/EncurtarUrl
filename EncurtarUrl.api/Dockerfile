FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copiar e restaurar dependências
COPY ["EncurtarUrl.api/EncurtarUrl.api.csproj", "EncurtarUrl.api/"]
COPY ["EncurtarUrl.Core/EncurtarUrl.Core.csproj", "EncurtarUrl.Core/"]
RUN dotnet restore "EncurtarUrl.api/EncurtarUrl.api.csproj"

# Build e publish
COPY . .
RUN dotnet publish "EncurtarUrl.api/EncurtarUrl.api.csproj" -c Release -o /app/publish

# Runtime image
FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app
COPY --from=build /app/publish .
EXPOSE 80
ENTRYPOINT ["dotnet", "EncurtarUrl.api.dll"]