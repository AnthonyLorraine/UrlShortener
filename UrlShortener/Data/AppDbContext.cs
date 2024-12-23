using Microsoft.EntityFrameworkCore;
using UrlShortener.Data.Models;

namespace UrlShortener.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions options) : base(options)
    {
        
        
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<UrlModel>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.ShortUrlSuffix)
                .IsUnique();
            entity.Property(e => e.ShortUrlSuffix)
                .IsRequired();
        });
    }

    public DbSet<UrlModel> Urls { get; set; }
}