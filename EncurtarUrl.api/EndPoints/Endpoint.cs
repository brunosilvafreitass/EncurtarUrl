using EncurtarUrl.api.Common;

namespace EncurtarUrl.api.EndPoints;

public static class Endpoint
{
    public static void MapEndPoints(this WebApplication app)
    {
        var endpoints = app.MapGroup("");

        // endpoints.MapGroup("/")
        // .WithTags("Healtch Check")
        // .MapGet("/", () => Results.Ok("API is running"));

        endpoints.MapGroup("")
        .WithTags("Shortened Urls")
        .MapEndpoint<UrlEndpoint>()
        .MapEndpoint<RedirectToOriginalUrlEndpoint>()
        .MapEndpoint<GetAllUrlsEndpoint>();
    }

    private static IEndpointRouteBuilder MapEndpoint<T>(this IEndpointRouteBuilder app)
    where T : IEndPoint
    {
        T.Map(app);
        return app;
    }
}