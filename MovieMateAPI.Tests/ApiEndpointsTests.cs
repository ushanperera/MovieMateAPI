using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using MovieMateAPI.Endpoints;
using MovieMateAPI.Models;
using MovieMateAPI.Services;

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

        /// <summary>
        ///  Check GetCompareMovies revert back all data after processing
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task GetCompareMovies_ReturnsOkResult_WithCorrectData()
        {
            // Arrange
            var mockMovieService = new Mock<IMovieService>();
            var movies = new List<(Movie movie, decimal? lowestPrice)>
            {
                (new Movie { Title = "Movie test 1", Year = "2021" }, 1234 ),
                (new Movie { Title = "Movie test 2", Year = "2022" }, null)
            };
            mockMovieService.Setup(s => s.GetLowestPriceMoviesPriceAsync()).ReturnsAsync(movies);

            // Act
            var result = await ApiEndpoints.GetLowestPriceMovies(mockMovieService.Object, _mockLoggerFactory.Object);

            // Assert
            var okResult = Assert.IsAssignableFrom<IValueHttpResult<IEnumerable<object>>>(result);
            var data = Assert.IsAssignableFrom<IEnumerable<object>>(okResult.Value);

            Assert.Equal(2, data.Count());
        }

    
    }
}