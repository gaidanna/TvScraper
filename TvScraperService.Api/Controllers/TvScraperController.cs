using Microsoft.AspNetCore.Mvc;
using TvScraperService.Api.Filters;
using TvScraperService.Core.Abstractions;

namespace TvScraperService.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TvScraperController : ControllerBase
    {
        private readonly ITvMazeScraperService _scraperService;
        public TvScraperController(ITvMazeScraperService scraperService)
        {
            _scraperService = scraperService;
        }

        [HttpGet]
        public async Task<IActionResult> GetShowCasts([FromQuery] PaginationFilter filter)
        {
            filter.ValidatePaginationParams();
            var pagedData = (await _scraperService.GetShows())
               .Skip((filter.PageNumber - 1) * filter.PageSize)
               .Take(filter.PageSize)
               .ToList();

            return Ok(pagedData);
        }
    }
}
