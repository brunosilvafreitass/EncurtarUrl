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

# Criar diretório SSL e gerar certificados auto-assinados
RUN mkdir -p /etc/nginx/ssl && \
    openssl req -x509 -nodes -days 365 -newkey rsa:2048 \
    -keyout /etc/nginx/ssl/key.pem \
    -out /etc/nginx/ssl/cert.pem \
    -subj "/C=BR/ST=State/L=City/O=Organization/CN=localhost"

# Configurar permissões corretamente
RUN chmod -R 755 /usr/share/nginx/html && \
    chown -R nginx:nginx /usr/share/nginx/html && \
    chmod 644 /etc/nginx/nginx.conf && \
    chmod 600 /etc/nginx/ssl/key.pem && \
    chmod 644 /etc/nginx/ssl/cert.pem

EXPOSE 80 443