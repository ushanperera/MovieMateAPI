using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Extensions.Logging;
using Moq;
using MovieMateAPI.Endpoints;
using MovieMateAPI.Models;
using MovieMateAPI.Repository;
using MovieMateAPI.Services;
using System.Text.Json;
using Xunit;

namespace MovieMateAPI.Tests
{
    public class ApiEndpointsTests
    {
        private readonly Mock<ILoggerFactory> _mockLoggerFactory;

        public ApiEndpointsTests()
        {
            _mockLoggerFactory = new Mock<ILoggerFactory>();
            var mockLogger = new Mock<ILogger>();
            _mockLoggerFactory.Setup(x => x.CreateLogger(It.IsAny<string>())).Returns(mockLogger.Object);
        }

        [Fact]
        public async Task GetCompareMovies_ReturnsOkResult_WithCorrectData()
        {
            // Arrange
            var mockMovieService = new Mock<IMovieService>();
            var movies = new List<(Movie movie, decimal? lowestPrice)>
            {
                (new Movie { Title = "Movie 1", Year = "2021", Poster = "poster1.jpg" }, 10.99m),
                (new Movie { Title = "Movie 2", Year = "2022", Poster = "poster2.jpg" }, null)
            };
            mockMovieService.Setup(s => s.GetCheaperMoviePriceAsync()).ReturnsAsync(movies);

            // Act
            var result = await Endpoints.ApiEndpoints.GetCompareMovies(mockMovieService.Object);

            // Assert
            var okResult = Assert.IsAssignableFrom<IValueHttpResult<IEnumerable<object>>>(result);
            var data = Assert.IsAssignableFrom<IEnumerable<object>>(okResult.Value);
            Assert.Equal(2, data.Count());
        }

    
    }
}