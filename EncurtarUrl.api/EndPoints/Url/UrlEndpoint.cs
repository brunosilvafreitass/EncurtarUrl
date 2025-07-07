using EncurtarUrl.api.Common;
using EncurtarUrl.api.Data;
using EncurtarUrl.api.Services;
using EncurtarUrl.Core.Handlers;
using EncurtarUrl.Core.Models;
using EncurtarUrl.Core.Requests;
using EncurtarUrl.Core.Responses;
using Microsoft.EntityFrameworkCore;

namespace EncurtarUrl.api.EndPoints;

public class UrlEndpoint : IEndPoint
{
    public static void Map(IEndpointRouteBuilder app)
    {
        app.MapPost("/", HandleAsync)
        .WithName("ShortenUrl")
        .WithSummary("Shorten a URL")
        .Produces<BaseResponse<Url?>>();

        // // ...existing code...
        // app.MapGet("/{shortCode}", async (string shortCode, AppDbContext db) =>
        // {
        //     var mapping = await db.Urls.FirstOrDefaultAsync(u => u.ShortenedCode == shortCode);
        //     if (mapping is null)
        //         return Results.NotFound();

        //     return Results.Redirect(mapping.OriginalUrl);
        // });
        // // ...existing code...

        // app.MapGet("/urls", (AppDbContext db) =>
        // {
        //     var urls = db.Urls.ToList();
        //     return TypedResults.Ok(urls);
        // });
    }



    private static async Task<IResult> HandleAsync(IUrlHandler handler, ShortenedUrlRequest request)
    {
        var result = await handler.CreateShortUrlAsync(request);

        return result.IsSuccess
            ? TypedResults.Created($"/{result.Data?.ShortenedCode}", result)
            : TypedResults.BadRequest(result.Data);
    }
}