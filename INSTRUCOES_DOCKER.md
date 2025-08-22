# Instruções para Resolver o Problema de Conexão

## Problema Identificado
O erro `file:///` indica que a configuração da URL da API não está sendo carregada corretamente no Blazor WebAssembly.

## Soluções Aplicadas

### 1. Correções no Código
✅ **Já aplicadas automaticamente:**
- Corrigido `appsettings.json` do WebWasm para usar `/api`
- Corrigido `nginx.conf` para proxy reverso correto
- Corrigido configuração de CORS na API
- Corrigido `Program.cs` do WebWasm para lidar com URLs relativas

### 2. Opções para Executar o Projeto

#### Opção A: Com Docker (Recomendado)

**Pré-requisito:** Instalar Docker Desktop
1. Baixe e instale o Docker Desktop: https://www.docker.com/products/docker-desktop/
2. Reinicie o computador após a instalação
3. Abra o Docker Desktop e aguarde inicializar
4. Execute no terminal:
   ```powershell
   docker --version  # Para verificar se está funcionando
   docker compose build --no-cache
   docker compose up -d
   ```
5. Acesse: http://localhost:5278

#### Opção B: Executar Localmente (Sem Docker)

**Para testar rapidamente sem Docker:**

1. **Executar a API:**
   ```powershell
   cd EncurtarUrl.api
   dotnet run
   ```
   A API ficará disponível em: http://localhost:5048

2. **Em outro terminal, executar o WebWasm:**
   ```powershell
   cd EncurtarUrl.WebWasm
   dotnet run
   ```
   O frontend ficará disponível em: http://localhost:5278

**Nota:** Para execução local, você precisará ajustar temporariamente o `appsettings.json` do WebWasm:
```json
{
  "BackEndUrl": "http://localhost:5048"
}
```

### 3. Verificação

Após executar (Docker ou local):
1. Acesse o frontend
2. Teste encurtar uma URL
3. Verifique se não há mais erros de conexão

### 4. Troubleshooting

**Se ainda houver problemas:**
- Verifique se ambos os serviços estão rodando
- Verifique os logs dos containers: `docker compose logs`
- Verifique se as portas 5048 e 5278 não estão sendo usadas por outros processos

**Logs úteis:**
```powershell
# Ver logs dos containers
docker compose logs api
docker compose logs web

# Ver status dos containers
docker compose ps
```