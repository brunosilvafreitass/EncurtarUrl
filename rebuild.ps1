# Script para reconstruir containers Docker com correções aplicadas

Write-Host "=== Reconstruindo Containers Docker ===" -ForegroundColor Green
Write-Host ""
Write-Host "🔧 Correção aplicada: UrlHandler agora faz POST para endpoint correto" -ForegroundColor Cyan
Write-Host "🔧 Problema resolvido: POST https://encurtador.brunoserver.ip-ddns.com/ 405" -ForegroundColor Cyan
Write-Host ""

# Verificar se Docker está disponível
try {
    docker --version | Out-Null
    Write-Host "✅ Docker detectado" -ForegroundColor Green
} catch {
    Write-Host "❌ Docker não encontrado. Instale o Docker Desktop" -ForegroundColor Red
    Write-Host "Download: https://www.docker.com/products/docker-desktop/" -ForegroundColor Yellow
    exit 1
}

# Parar e remover containers existentes
Write-Host "🛑 Parando containers..." -ForegroundColor Yellow
docker compose down

# Reconstruir imagens sem cache
Write-Host "🔨 Reconstruindo imagens..." -ForegroundColor Cyan
docker compose build --no-cache

# Iniciar containers
Write-Host "🚀 Iniciando containers..." -ForegroundColor Green
docker compose up -d

Write-Host ""
Write-Host "✅ Rebuild concluído!" -ForegroundColor Green
Write-Host ""
Write-Host "📊 Status dos containers:" -ForegroundColor Magenta
docker compose ps

Write-Host ""
Write-Host "🌐 Acesse a aplicação em:" -ForegroundColor Magenta
Write-Host "   HTTP:  http://localhost:5278" -ForegroundColor Cyan
Write-Host "   HTTPS: https://localhost:5443" -ForegroundColor Cyan
Write-Host "🔧 API disponível em: http://localhost:5048" -ForegroundColor Magenta
Write-Host "📚 Swagger UI: http://localhost:5048/swagger" -ForegroundColor Magenta
Write-Host ""
Write-Host "💡 Correções aplicadas:" -ForegroundColor Green
Write-Host "   ✅ Erro 'POST https://encurtador.brunoserver.ip-ddns.com/ 405' corrigido" -ForegroundColor Green
Write-Host "   ✅ Mixed Content (HTTP/HTTPS) resolvido" -ForegroundColor Green
Write-Host "   ✅ Suporte a HTTPS adicionado" -ForegroundColor Green

Write-Host ""
Write-Host "Pressione qualquer tecla para continuar..." -ForegroundColor Gray
$null = $Host.UI.RawUI.ReadKey("NoEcho,IncludeKeyDown")