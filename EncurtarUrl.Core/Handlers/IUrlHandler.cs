using EncurtarUrl.Core.Models;
using EncurtarUrl.Core.Requests;
using EncurtarUrl.Core.Responses;

namespace EncurtarUrl.Core.Handlers
{
    public interface IUrlHandler
    {
        Task<BaseResponse<Url?>> CreateShortUrlAsync(ShortenedUrlRequest request);

        // Task<BaseResponse<Url?>> RedirectToOriginalUrl(string shortCode);
    }
}