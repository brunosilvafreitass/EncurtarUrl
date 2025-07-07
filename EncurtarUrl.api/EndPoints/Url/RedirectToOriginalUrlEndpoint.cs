using EncurtarUrl.api.Common;
using EncurtarUrl.Core.Handlers;
using EncurtarUrl.Core.Models;
using EncurtarUrl.Core.Responses;

namespace EncurtarUrl.api.EndPoints;

public class RedirectToOriginalUrlEndpoint : IEndPoint
{
    public static void Map(IEndpointRouteBuilder app)
    {
        app.MapGet("/{shortCode}", HandlerAsync)
        .Produces<BaseResponse<Url?>>();
    }

    private static async Task<IResult> HandlerAsync(IUrlHandler handler, string shortCode)
    {
        var result = await handler.RedirectToOriginalUrl(shortCode);

        return result.IsSuccess && result.Data?.OriginalUrl is not null
            ? Results.Redirect(result.Data.OriginalUrl)
            : TypedResults.NotFound(result.Message);
    }


}
