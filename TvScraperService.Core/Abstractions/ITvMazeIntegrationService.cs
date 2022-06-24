using TvScraperService.Core.Models;

namespace TvScraperService.Core.Abstractions
{
    public interface ITvMazeIntegrationService
    {
        Task<List<Show>> LoadShows();
    }
}
