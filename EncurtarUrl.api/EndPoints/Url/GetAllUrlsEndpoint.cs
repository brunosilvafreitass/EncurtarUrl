using EncurtarUrl.api.Common;
using EncurtarUrl.Core.Handlers;
using EncurtarUrl.Core.Models;
using EncurtarUrl.Core.Responses;

namespace EncurtarUrl.api.EndPoints;

public class GetAllUrlsEndpoint : IEndPoint
{
    public static void Map(IEndpointRouteBuilder app)
    {
        app.MapGet("/urls", HandleAsync)
            .Produces<BaseResponse<List<Url?>>>();
    }
    private static async Task<IResult> HandleAsync(IUrlHandler handler)
    {
        var result = await handler.GetAllUrlsAsync();

        return result.IsSuccess
            ? TypedResults.Ok(result)
            : TypedResults.NotFound(result.Message);
    }
}
