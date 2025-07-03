using EncurtarUrl.api.Data;
using EncurtarUrl.api.Services;
using EncurtarUrl.Core.Handlers;
using EncurtarUrl.Core.Models;
using EncurtarUrl.Core.Requests;
using EncurtarUrl.Core.Responses;

namespace EncurtarUrl.api.Handlers
{
    public class Urlhandler(AppDbContext context, UrlShortenedService shortenedService) : IUrlHandler
    {
        public Task<BaseResponse<Url?>> CreateShortUrlAsync(ShortenedUrlRequest request)
        {
            throw new NotImplementedException();
        }
    }
}