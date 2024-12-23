using Microsoft.AspNetCore.Mvc;
using UrlShortener.Services;

namespace UrlShortener.Endpoints;

public static class UrlsEndpoint
{
    public static void MapUrlsEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("", async (
            [FromQuery(Name = "url")] string originalUrl,
            [FromServices] UrlsService service) => 
            await service.CreateShortUrlAsync(originalUrl));
        
        app.MapGet("/{code}", async (
            string code,
            [FromServices] UrlsService service) =>
        {
            try
            {
                var url = await service.RetrieveOriginalUrlAsync(code);
                return Results.Ok(url);
            }
            catch
            {
                return Results.NotFound();
            }
        });

        app.MapGet("/Maintenance/CleanUrls", async ([FromServices] UrlsService service) =>
        {
            await service.CleanupUrlsAsync();
            return Results.Ok("All Urls have been cleaned.");
        });
    }
}