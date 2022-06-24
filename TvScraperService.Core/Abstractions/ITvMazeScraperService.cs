using TvScraperService.Core.Models;

namespace TvScraperService.Core.Abstractions
{
    public interface ITvMazeScraperService
    {
        Task<List<Show>> GetShows();
    }
}
