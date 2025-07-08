using EncurtarUrl.Core.Handlers;
using EncurtarUrl.Core.Models;
using EncurtarUrl.Core.Requests;
using EncurtarUrl.Core.Responses;

namespace EncurtarUrl.Web.Handlers;

public class UrlHandler(IHttpClientFactory httpClientFactory) : IUrlHandler
{
    private readonly HttpClient _httpClient = httpClientFactory.CreateClient("EncurtarUrlApi");
    public async Task<BaseResponse<Url?>> CreateShortUrlAsync(ShortenedUrlRequest request)
    {
        var result = await _httpClient.PostAsJsonAsync("api/v1/shortened", request);
        return await result.Content.ReadFromJsonAsync<BaseResponse<Url?>>() ?? new BaseResponse<Url?>(null, 400, "Failed to create short URL");
    }

    public Task<BaseResponse<List<Url?>>> GetAllUrlsAsync()
    {
        throw new NotImplementedException();
    }

    public Task<BaseResponse<Url?>> RedirectToOriginalUrl(string shortCode)
    {
        throw new NotImplementedException();
    }
}
