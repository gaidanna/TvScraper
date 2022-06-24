using TvScraperService.Core.Models;

namespace TvScraperService.Core.Abstractions
{
    public interface ITvShowRepository
    {
        Task<List<Show>> GetShows();

        Task<List<Show>> SetShows(List<Show> shows);
    }
}
