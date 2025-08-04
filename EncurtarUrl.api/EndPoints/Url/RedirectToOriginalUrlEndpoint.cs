using EncurtarUrl.api.Common;
using EncurtarUrl.Core.Handlers;
using EncurtarUrl.Core.Models;
using EncurtarUrl.Core.Responses;
using Microsoft.AspNetCore.Http.HttpResults;

namespace EncurtarUrl.api.EndPoints;

public class RedirectToOriginalUrlEndpoint : IEndPoint
{
    public static void Map(IEndpointRouteBuilder app)
    {
        app.MapGet("/{shortCode}", HandlerAsync)
        .Produces<BaseResponse<Url?>>(200)
        .Produces(302) // Redirecionamento
        .Produces(404); // Não encontrado
    }

    private static async Task<IResult> HandlerAsync(IUrlHandler handler, string shortCode)
    {
        var result = await handler.RedirectToOriginalUrl(shortCode);

        if (result.IsSuccess && result.Data?.OriginalUrl is not null)
        {
            // Garantir que a URL tenha o protocolo
            string originalUrl = result.Data.OriginalUrl;
            if (!originalUrl.StartsWith("http://") && !originalUrl.StartsWith("https://"))
            {
                originalUrl = "https://" + originalUrl;
            }
            
            // Redirecionar para a URL original
            return Results.Redirect(originalUrl, false); // false = redirecionamento temporário (302)
        }
        else
        {
            // Retornar 404 se a URL não for encontrada
            return TypedResults.NotFound(new { message = result.Message ?? "URL não encontrada" });
        }
    }
}
