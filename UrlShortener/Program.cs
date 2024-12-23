using Microsoft.EntityFrameworkCore;
using UrlShortener.Data;
using UrlShortener.Endpoints;
using UrlShortener.Services;

namespace UrlShortener;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.Services.AddDbContext<AppDbContext>(o =>
        {
            o.UseSqlite("Data Source=UrlShortener.db");
        });
        builder.Services.AddScoped<UrlsService>();
        builder.Services.AddAuthorization();
        builder.Services.AddOpenApi();
        

        var app = builder.Build();
        
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
        }
        
        app.UseHttpsRedirection();
        app.UseAuthorization();
        
        app.MapUrlsEndpoints();
        
        app.Run();
    }
}