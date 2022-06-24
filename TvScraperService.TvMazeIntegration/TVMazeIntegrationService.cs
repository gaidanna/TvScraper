using AutoMapper;
using Newtonsoft.Json;
using TvScraperService.Core.Abstractions;
using TvScraperService.Core.Models;
using TvScraperService.TvMazeIntegration.Models;

namespace TvScraperService.TvMazeIntegration
{
    public class TestTVMazeIntegrationService : BackgroundService
    {
        private readonly ILogger<TestTVMazeIntegrationService> _logger;
        private readonly ITvShowRepository _tvScraperRepository;
        private readonly IMapper _mapper;
        public TestTVMazeIntegrationService(ILogger<TestTVMazeIntegrationService> logger,
            ITvShowRepository tvScraperRepository,
            IMapper mapper)
        {
            _logger = logger;
            _tvScraperRepository = tvScraperRepository;
            _mapper = mapper;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                await LoadShows();
            }
        }

        public async Task LoadShows()
        {
            var pageNumber = 0;
            string URL = "https://api.tvmaze.com/shows";


            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(URL);


            var shows = new List<ShowInfo>();
            var casts = new List<CastInfo>();
            var showResults = new List<Show>();

            while (true)
            {
                string urlParameters = $"?page={pageNumber}";
                HttpResponseMessage response = await client.GetAsync(URL + urlParameters);
                if (!response.IsSuccessStatusCode)
                {
                    break;
                }

                shows.AddRange(await response.Content.ReadAsAsync<IEnumerable<ShowInfo>>());
                showResults = _mapper.Map<List<Show>>(shows);
                foreach (var showResult in showResults)
                {
                    //casts.AddRange(await ReadShowCast(showResult.Id));
                    var cast = await ReadShowCast(showResult.Id);

                    showResult.CastActors = _mapper.Map<List<Actor>>(cast);
                }

                pageNumber++;
            }
            //TODO: mapper
            //var showResults = _mapper.Map<List<Show>>(shows);
            await _tvScraperRepository.SetShows(showResults);
        }

        public async Task<List<CastInfo>> ReadShowCast(int showId)
        {
            string URL = "https://api.tvmaze.com/shows";
            List<CastInfo> casts = null;

            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(URL);

            string castParams = $"/{showId}/cast";
            HttpResponseMessage response = await client.GetAsync(URL + castParams);
            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                casts = JsonConvert.DeserializeObject<List<CastInfo>>(jsonString,
                   new JsonSerializerSettings() { DateFormatString = "yyyy-MM-dd" });
            }
            casts.ForEach(x => x.ShowId = showId);
            return casts;
        }
    }
}