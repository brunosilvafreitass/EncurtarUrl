FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY ["EncurtarUrl.WebWasm/EncurtarUrl.WebWasm.csproj", "EncurtarUrl.WebWasm/"]
COPY ["EncurtarUrl.Core/EncurtarUrl.Core.csproj", "EncurtarUrl.Core/"]
RUN dotnet restore "EncurtarUrl.WebWasm/EncurtarUrl.WebWasm.csproj"

COPY . .
RUN dotnet publish "EncurtarUrl.WebWasm/EncurtarUrl.WebWasm.csproj" -c Release -o /app/publish

FROM nginx:alpine
WORKDIR /usr/share/nginx/html

# Copiar arquivos e configurar permissões
COPY --from=build /app/publish/wwwroot .
COPY nginx.conf /etc/nginx/nginx.conf

# Configurar permissões corretamente
RUN chmod -R 755 /usr/share/nginx/html && \
    chown -R nginx:nginx /usr/share/nginx/html && \
    chmod 644 /etc/nginx/nginx.conf

EXPOSE 80