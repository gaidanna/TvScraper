using Microsoft.EntityFrameworkCore;
using TvScraperService.Core.Abstractions;
using TvScraperService.Core.Models;

namespace TvScraperService.Infrastructure.Repository
{
    public class TvShowRepository : ITvShowRepository
    {
        private readonly TvScraperDbContext _scraperDbContext;
        public TvShowRepository(TvScraperDbContext scraperDbContext)
        {
            _scraperDbContext = scraperDbContext;
        }
        public async Task<List<Show>> GetShows()
        {
            return await _scraperDbContext.Shows
                .Include(c=> c.Cast.OrderByDescending(x => x.Birthday))
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<List<Show>> SetShows(List<Show> shows)
        {
            foreach (var show in shows)
            {
                for (int i = 0; i < show.Cast.Count; i++)
                {
                    Actor? actor = show.Cast[i];
                    var existingActor = _scraperDbContext.Actors
                        .FirstOrDefault(b => b.TVMazeId == actor.TVMazeId);
                    
                    if (existingActor != null)
                    {
                        show.Cast[i] = existingActor;
                    }
                }

                await _scraperDbContext.Shows.AddAsync(show);
                await _scraperDbContext.SaveChangesAsync();
            }

            return shows;
        }
    }
}
