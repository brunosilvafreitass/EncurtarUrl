# Correção do Erro Mixed Content (HTTP/HTTPS)

## Problema Identificado
O erro `Mixed Content: The page at 'https://encurtador.brunoserver.ip-ddns.com/' was loaded over HTTPS, but requested an insecure resource 'http://encurtador.brunoserver.ip-ddns.com/api/'` ocorre quando:

- A aplicação é servida via HTTPS
- Mas tenta fazer requisições HTTP para a API
- Navegadores bloqueiam requisições HTTP em páginas HTTPS por segurança

## Correções Aplicadas

### 1. Configuração HTTPS no Nginx
✅ **Arquivo:** `nginx.conf`
- Adicionado redirecionamento HTTP → HTTPS
- Configurado servidor HTTPS na porta 443
- Adicionadas configurações SSL básicas

### 2. Certificados SSL Auto-assinados
✅ **Arquivo:** `EncurtarUrl.WebWasm/Dockerfile`
- Geração automática de certificados SSL para desenvolvimento
- Configuração de permissões adequadas
- Exposição da porta 443

### 3. Mapeamento de Portas
✅ **Arquivo:** `compose.yml`
- Adicionada porta 5443 para HTTPS
- Mantida porta 5278 para HTTP (redirecionamento)

## Como Aplicar as Correções

### Opção 1: Rebuild Completo (Recomendado)
```powershell
.\rebuild.ps1
```

### Opção 2: Comandos Manuais
```powershell
# Parar containers
docker compose down

# Rebuild sem cache
docker compose build --no-cache

# Iniciar com novas configurações
docker compose up -d
```

## Acesso à Aplicação

Após aplicar as correções:

- **HTTP:** http://localhost:5278 (redireciona para HTTPS)
- **HTTPS:** https://localhost:5443 ⭐ **Use esta URL**
- **API:** http://localhost:5048
- **Swagger:** http://localhost:5048/swagger

## Verificação

1. Acesse https://localhost:5443
2. Aceite o certificado auto-assinado (aviso de segurança é normal)
3. Teste o encurtamento de URL
4. Verifique que não há mais erros de Mixed Content no console

## Para Produção

Para ambiente de produção real:

1. **Substitua certificados auto-assinados** por certificados válidos (Let's Encrypt, etc.)
2. **Configure domínio real** no nginx
3. **Use proxy reverso** (Cloudflare, nginx externo, etc.)

### Exemplo com Let's Encrypt:
```bash
# Instalar certbot
sudo apt install certbot python3-certbot-nginx

# Obter certificado
sudo certbot --nginx -d seu-dominio.com
```

## Troubleshooting

### Erro "Certificado não confiável"
- **Normal para desenvolvimento** com certificados auto-assinados
- Clique em "Avançado" → "Continuar para localhost (não seguro)"

### Porta 5443 não acessível
- Verifique se o container está rodando: `docker compose ps`
- Verifique logs: `docker compose logs web`
- Verifique se a porta não está em uso: `netstat -an | findstr 5443`

### Mixed Content ainda ocorre
- Limpe cache do navegador
- Verifique se está acessando via HTTPS (porta 5443)
- Verifique logs do nginx: `docker compose logs web`

## Arquitetura da Solução

```
Navegador (HTTPS) → nginx:443 → Blazor WASM
                 ↓
              nginx:80 → API:80 (proxy reverso)
```

- **Frontend:** Servido via HTTPS pelo nginx
- **API:** Acessada via proxy reverso (mesmo protocolo)
- **Sem Mixed Content:** Todas as requisições mantêm o protocolo HTTPS