using AutoFixture;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using TvScraperService.Core.Abstractions;
using TvScraperService.Core.Models;
using TvScraperService.Core.Services;
using Xunit;

namespace TvScraperService.Tests
{
    public class GetTvShowCastTests
    {
        private readonly Mock<ITvShowRepository> _tvShowRepository;
        private readonly Fixture _fixture;
        private readonly ITvMazeScraperService _userService;
        public GetTvShowCastTests()
        {
            _fixture = new Fixture();
            _fixture.Behaviors.Remove(new ThrowingRecursionBehavior());
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
            _tvShowRepository = new Mock<ITvShowRepository>();
            _userService = new TvMazeScraperService(_tvShowRepository.Object);
        }

        [Fact]
        public async Task GetShowCastSuccessfully()
        {
            //Arrange
            var shows = _fixture.Create<List<Show>>();

            _tvShowRepository.Setup(r => r.GetShows())
                .ReturnsAsync(shows);

            //Act
            await _userService.GetShows();

            //Assert
            _tvShowRepository.Verify(m => m.GetShows(), Times.Once);
        }
    }
}