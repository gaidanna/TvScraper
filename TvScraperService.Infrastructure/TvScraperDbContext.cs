using Microsoft.EntityFrameworkCore;
using TvScraperService.Core.Models;

namespace TvScraperService.Infrastructure;
public class TvScraperDbContext : DbContext
{
    public TvScraperDbContext(DbContextOptions<TvScraperDbContext> options)
        : base(options)
    {
    }
    public DbSet<Actor> Actors { get; set; }
    public DbSet<Show> Shows { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Show>()
            .Property(b => b.TVMazeId)
            .IsRequired();

        modelBuilder.Entity<Show>()
            .Property(b => b.Name)
            .IsRequired();

        modelBuilder.Entity<Actor>()
            .Property(b => b.TVMazeId)
            .IsRequired();
    }
}
