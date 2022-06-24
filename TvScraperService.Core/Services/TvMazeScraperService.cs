using TvScraperService.Core.Abstractions;
using TvScraperService.Core.Models;

namespace TvScraperService.Core.Services
{
    public class TvMazeScraperService : ITvMazeScraperService
    {
        private readonly ITvShowRepository _scraperRepository;
        public TvMazeScraperService(ITvShowRepository scraperRepository)
        {
            _scraperRepository = scraperRepository;
        }

        public async Task<List<Show>> GetShows()
        {
            return await _scraperRepository.GetShows();
        }
    }
}
