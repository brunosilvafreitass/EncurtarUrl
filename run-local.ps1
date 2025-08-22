# Script para executar o projeto localmente (sem Docker)

Write-Host "=== Executando Projeto Localmente (Sem Docker) ===" -ForegroundColor Green
Write-Host ""

# Verificar se o .NET está instalado
try {
    $dotnetVersion = dotnet --version
    Write-Host "✅ .NET detectado: $dotnetVersion" -ForegroundColor Green
} catch {
    Write-Host "❌ .NET não encontrado. Instale o .NET 9.0 SDK" -ForegroundColor Red
    Write-Host "Download: https://dotnet.microsoft.com/download" -ForegroundColor Yellow
    exit 1
}

Write-Host ""
Write-Host "📝 Ajustando configuração para execução local..." -ForegroundColor Yellow

# Backup do appsettings.json atual
$appsettingsPath = "EncurtarUrl.WebWasm\wwwroot\appsettings.json"
$backupPath = "EncurtarUrl.WebWasm\wwwroot\appsettings.json.backup"

if (Test-Path $appsettingsPath) {
    Copy-Item $appsettingsPath $backupPath -Force
    Write-Host "✅ Backup criado: $backupPath" -ForegroundColor Green
}

# Criar appsettings.json para execução local
$localConfig = @'
{
  "BackEndUrl": "http://localhost:5048"
}
'@

$localConfig | Out-File -FilePath $appsettingsPath -Encoding UTF8
Write-Host "✅ Configuração ajustada para localhost" -ForegroundColor Green

Write-Host ""
Write-Host "🚀 Iniciando API..." -ForegroundColor Cyan

# Iniciar API em background
Start-Process powershell -ArgumentList "-NoExit", "-Command", "cd '$PWD\EncurtarUrl.api'; Write-Host 'API iniciando em http://localhost:5048' -ForegroundColor Green; dotnet run"

Write-Host "⏳ Aguardando API inicializar..." -ForegroundColor Yellow
Start-Sleep -Seconds 5

Write-Host ""
Write-Host "🌐 Iniciando Frontend..." -ForegroundColor Cyan

# Iniciar Frontend
Start-Process powershell -ArgumentList "-NoExit", "-Command", "cd '$PWD\EncurtarUrl.WebWasm'; Write-Host 'Frontend iniciando em http://localhost:5278' -ForegroundColor Green; dotnet run"

Write-Host ""
Write-Host "✅ Projeto iniciado com sucesso!" -ForegroundColor Green
Write-Host ""
Write-Host "📱 Frontend: http://localhost:5278" -ForegroundColor Magenta
Write-Host "🔧 API: http://localhost:5048" -ForegroundColor Magenta
Write-Host ""
Write-Host "💡 Para parar os serviços, feche as janelas do PowerShell que abriram" -ForegroundColor Yellow
Write-Host "💡 Para restaurar configuração Docker: copy appsettings.json.backup appsettings.json" -ForegroundColor Yellow
Write-Host ""
Write-Host "Pressione qualquer tecla para continuar..." -ForegroundColor Gray
$null = $Host.UI.RawUI.ReadKey("NoEcho,IncludeKeyDown")