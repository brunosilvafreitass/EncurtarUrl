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
                    ShortenedUrl = $"{Configuration.BackEndUrl}/{shortCode}",
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

    }
}