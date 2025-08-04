using EncurtarUrl.api.Data;
using EncurtarUrl.api.Services;
using EncurtarUrl.Core;
using EncurtarUrl.Core.Handlers;
using EncurtarUrl.Core.Models;
using EncurtarUrl.Core.Requests;
using EncurtarUrl.Core.Responses;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace EncurtarUrl.api.Handlers
{
    public class Urlhandler(AppDbContext context, UrlShortenedService shortenedService) : IUrlHandler
    {
        public async Task<BaseResponse<Url?>> CreateShortUrlAsync(ShortenedUrlRequest request)
        {
            if (string.IsNullOrEmpty(request.OriginalUrl))
            {
                return new BaseResponse<Url?>(null, 400, "URL não pode ser vazia");
            }

            // Garantir que a URL tenha o protocolo
            string originalUrl = request.OriginalUrl;
            if (!originalUrl.StartsWith("http://") && !originalUrl.StartsWith("https://"))
            {
                originalUrl = "https://" + originalUrl;
            }

            try
            {
                int hashCode = Math.Abs(originalUrl.GetHashCode());
                var shortCode = shortenedService.GenerateShortenedCode(hashCode);

                // Verificar se já existe uma URL com o mesmo código
                var existingUrl = await context.Urls.FirstOrDefaultAsync(u => u.ShortenedCode == shortCode);
                if (existingUrl != null)
                {
                    // Se já existe, verificar se é a mesma URL original
                    if (existingUrl.OriginalUrl == originalUrl)
                    {
                        return new BaseResponse<Url?>(existingUrl, 200, "URL já foi encurtada anteriormente");
                    }
                    else
                    {
                        // Se for uma URL diferente, gerar um novo código
                        shortCode = shortenedService.GenerateShortenedCode(hashCode + DateTime.Now.Millisecond);
                    }
                }

                var mappedUrl = new Url
                {
                    OriginalUrl = originalUrl,
                    ShortenedCode = shortCode,
                    ShortenedUrl = $"{Configuration.BackEndUrl}/api/v1/shortened/{shortCode}",
                    ClickCount = 0,
                    CreatedAt = DateTime.UtcNow.AddHours(-3)
                };

                await context.Urls.AddAsync(mappedUrl);
                await context.SaveChangesAsync();

                return new BaseResponse<Url?>(mappedUrl, 201, "URL encurtada com sucesso");
            }
            catch (Exception ex)
            {
                return new BaseResponse<Url?>(null, 500, $"Erro ao encurtar a URL: {ex.Message}");
            }
        }

        public async Task<BaseResponse<List<Url?>>> GetAllUrlsAsync()
        {
            try
            {
                var urls = await context.Urls.ToListAsync();

                if (urls == null || urls.Count == 0)
                {
                    return new BaseResponse<List<Url?>>(null, 404, "Nenhuma URL encontrada");
                }

                return new BaseResponse<List<Url?>>(urls!, 200, "URLs encontradas");
            }
            catch (Exception)
            {

                return new BaseResponse<List<Url?>>(null, 500, "Erro ao buscar URLs");
            }
        }

        public async Task<BaseResponse<Url?>> RedirectToOriginalUrl(string shortCode)
        {
            var mapping = await context.Urls.FirstOrDefaultAsync(u => u.ShortenedCode == shortCode);
            if (mapping != null)
            {
                mapping.ClickCount++;
                await context.SaveChangesAsync();
            }
            else
                return new BaseResponse<Url?>(null, 404, "URL não encontrada");

            return new BaseResponse<Url?>(mapping, 200, "URL encontrada");
        }
    }
}