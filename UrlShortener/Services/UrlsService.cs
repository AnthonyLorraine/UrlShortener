using System.Globalization;
using System.Text;
using Microsoft.EntityFrameworkCore;
using UrlShortener.Data;
using UrlShortener.Data.Models;

namespace UrlShortener.Services;

public class UrlsService(AppDbContext dbContext)
{
    private const string BaseUrl = "https://www.rishu.dev/";
    
    public async Task<string> CreateShortUrlAsync(string originalUrl)
    {
        var urlModel = new UrlModel
        {
            OriginalUrl = originalUrl,
            ShortUrlSuffix = await GenerateShortUrlAsync()
        };
        
        while (true)
        {
            try
            {
                await dbContext.Urls.AddAsync(urlModel);
                await dbContext.SaveChangesAsync();
                break;
            }
            catch
            {
                // ignored
            }
        }

        return BaseUrl + urlModel.ShortUrlSuffix;
    }

    public async Task<string> RetrieveOriginalUrlAsync(string shortUrl)
    {
        var url = await dbContext.Urls.FirstAsync(u => u.ShortUrlSuffix == shortUrl);

        return url.OriginalUrl;
    }


    public async Task CleanupUrlsAsync()
    {
        var urlsToClean = await dbContext.Urls
            .Where(u => u.CreatedAt < DateTime.UtcNow.AddHours(-24))
            .ToListAsync();
        dbContext.Urls.RemoveRange(urlsToClean);
        await dbContext.SaveChangesAsync();
    }
    private async Task<string> GenerateShortUrlAsync()
    {
        const string characters = "abcdefghijklmnopqrstuvwxyz";
        const int shortCodeLength = 5;

        var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        var timestamp = (DateTime.UtcNow - epoch).TotalSeconds.ToString(CultureInfo.InvariantCulture);

        var random = new Random();
        var shortCode = new StringBuilder(shortCodeLength);

        for (var i = 0; i < shortCodeLength; i++)
        {
            var charIndex = (int.Parse(timestamp[i % timestamp.Length].ToString()) + random.Next(0, characters.Length)) % characters.Length;
            var selectedChar = characters[charIndex];

            
            if (random.Next(0, 2) == 1)
            {
                selectedChar = char.ToUpperInvariant(selectedChar);
            }

            shortCode.Append(selectedChar);
        }

        return await Task.FromResult(shortCode.ToString());
    }

}