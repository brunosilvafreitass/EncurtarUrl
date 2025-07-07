using EncurtarUrl.api.Data;
using EncurtarUrl.api.Services;
using EncurtarUrl.Core;
using EncurtarUrl.Core.Handlers;
using EncurtarUrl.Core.Models;
using EncurtarUrl.Core.Requests;
using EncurtarUrl.Core.Responses;
using Microsoft.EntityFrameworkCore;

namespace EncurtarUrl.api.Handlers
{
    public class Urlhandler(AppDbContext context, UrlShortenedService shortenedService) : IUrlHandler
    {
        public async Task<BaseResponse<Url?>> CreateShortUrlAsync(ShortenedUrlRequest request)
        {
            if (string.IsNullOrEmpty(request.OriginalUrl))
            {
                return new BaseResponse<Url?>(null, 500, "Url nao pode ser vazia");
            }

            try
            {
                int hashCode = Math.Abs(request.OriginalUrl.GetHashCode());
                var shortCode = shortenedService.GenerateShortenedCode(hashCode);

                var mappedUrl = new Url
                {
                    OriginalUrl = request.OriginalUrl,
                    ShortenedCode = shortCode,
                    ShortenedUrl = $"{Configuration.BackEndUrl}/api/v1/shortened/{shortCode}",
                    ClickCount = 0,
                    CreatedAt = DateTime.UtcNow.AddHours(-3)
                };

                await context.Urls.AddAsync(mappedUrl);
                await context.SaveChangesAsync();

                return new BaseResponse<Url?>(mappedUrl, 200, "Url encurtada com sucesso");

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