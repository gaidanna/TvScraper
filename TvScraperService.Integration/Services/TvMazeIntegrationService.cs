using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using TvScraperService.Core.Abstractions;
using TvScraperService.Core.Models;
using TvScraperService.Integration.Models;

namespace TvScraperService.Integration.Services
{
    public class TvMazeIntegrationService : BackgroundService, ITvMazeIntegrationService
    {
        private readonly string Url = "https://api.tvmaze.com/shows";
        private readonly IServiceProvider _serviceProvider;
        private readonly IMapper _mapper;
        private readonly ILogger<TvMazeIntegrationService> _logger;
        public TvMazeIntegrationService(IServiceProvider serviceProvider,
            IMapper mapper,
            ILogger<TvMazeIntegrationService> logger)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
            _mapper = mapper;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Shows and casts are loading. Please wait.");

            await LoadShows();

            _logger.LogInformation("Shows and casts have been loaded successfully.");
        }
        public async Task<List<Show>> LoadShows()
        {
            var pageNumber = 0;
            var showDtos = new List<ShowDto>();
            var shows = new List<Show>();
            
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(Url);

            while (true)
                {
                    string urlParameters = $"?page={pageNumber}";
                    HttpResponseMessage response = await client.GetAsync(Url + urlParameters);
                    if (!response.IsSuccessStatusCode)
                    {
                        break;
                    }

                    showDtos.AddRange(await response.Content.ReadAsAsync<IEnumerable<ShowDto>>());
                    shows = _mapper.Map<List<Show>>(showDtos);

                    foreach (var show in shows)
                    {
                        var cast = await LoadShowCast(show.TVMazeId);

                        show.Cast = _mapper.Map<List<Actor>>(cast);
                    }
                    pageNumber++;
                }
            using (IServiceScope scope = _serviceProvider.CreateScope())
            {
                var tvShowRepository = scope.ServiceProvider.GetRequiredService<ITvShowRepository>();
                return await tvShowRepository.SetShows(shows);
            }
        }

        private async Task<List<PersonDto>> LoadShowCast(int tvMazeShowId)
        {
            List<CastDto> castDtos = new List<CastDto>();
            
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(Url);
            string castParams = $"/{tvMazeShowId}/cast";
            HttpResponseMessage response = await client.GetAsync(Url + castParams);

            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                castDtos = JsonConvert.DeserializeObject<List<CastDto>>(jsonString,
                   new JsonSerializerSettings() { DateFormatString = "yyyy-MM-dd" });
            }
            var persons = castDtos.Select(x => x.Person).DistinctBy(x => x.Id).ToList();
            
            return persons;
        }
        
    }
}
