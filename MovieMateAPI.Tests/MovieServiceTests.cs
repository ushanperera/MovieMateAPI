using Microsoft.Extensions.Configuration;
using Moq;
using Moq.Protected;
using MovieMateAPI.Models;
using MovieMateAPI.Services;
using System.Net;
using System.Text.Json;
using Xunit;

namespace MovieMateAPI.Tests.Services
{
    public class MovieServiceTests
    {
        private readonly Mock<IHttpClientFactory> _mockHttpClientFactory;
        private readonly Mock<IConfiguration> _mockConfig;
        private readonly Mock<HttpMessageHandler> _mockHttpMessageHandler;
        private readonly MovieService _movieService;

        public MovieServiceTests()
        {
            _mockHttpClientFactory = new Mock<IHttpClientFactory>();
            _mockConfig = new Mock<IConfiguration>();
            _mockHttpMessageHandler = new Mock<HttpMessageHandler>();

            var httpClient = new HttpClient(_mockHttpMessageHandler.Object);
            _mockHttpClientFactory.Setup(_ => _.CreateClient("WebjetClient")).Returns(httpClient);

            _mockConfig.Setup(c => c["WebjetApi:BaseUrl"]).Returns("http://testapi/");
            _mockConfig.Setup(c => c["WebjetApi:ApiToken"]).Returns("test-token");

            _movieService = new MovieService(_mockConfig.Object, _mockHttpClientFactory.Object);
        }

        [Fact]
        public async Task GetMoviesWithCheapestPriceAsync_ReturnsMoviesWithLowestPrices()
        {
            // Arrange
            var cinemaWorldMovies = new MovieResponse
            {
                Movies = new List<Movie>
                {
                    new Movie { ID = "cw1", Title = "Movie 1" },
                    new Movie { ID = "cw2", Title = "Movie 2" }
                }
            };

            var filmWorldMovies = new MovieResponse
            {
                Movies = new List<Movie>
                {
                    new Movie { ID = "fw1", Title = "Movie 1" },
                    new Movie { ID = "fw3", Title = "Movie 3" }
                }
            };

            var cinemaWorldDetails1 = new MovieDetails { Price = "10.50" };
            var cinemaWorldDetails2 = new MovieDetails { Price = "15.00" };
            var filmWorldDetails1 = new MovieDetails { Price = "9.99" };
            var filmWorldDetails3 = new MovieDetails { Price = "20.00" };

            SetupHttpResponse("http://testapi/cinemaworld/movies", cinemaWorldMovies);
            SetupHttpResponse("http://testapi/filmworld/movies", filmWorldMovies);
            SetupHttpResponse("http://testapi/cinemaworld/movie/cw1", cinemaWorldDetails1);
            SetupHttpResponse("http://testapi/cinemaworld/movie/cw2", cinemaWorldDetails2);
            SetupHttpResponse("http://testapi/filmworld/movie/fw1", filmWorldDetails1);
            SetupHttpResponse("http://testapi/filmworld/movie/fw3", filmWorldDetails3);

            // Act
            var result = await _movieService.GetMoviesWithCheapestPriceAsync();

            // Assert
            Assert.Equal(3, result.Count);
            Assert.Equal("Movie 1", result[0].movie.Title);
            Assert.Equal(9.99M, result[0].lowestPrice);
            Assert.Equal("Movie 2", result[1].movie.Title);
            Assert.Equal(15.00M, result[1].lowestPrice);
            Assert.Equal("Movie 3", result[2].movie.Title);
            Assert.Equal(20.00M, result[2].lowestPrice);
        }

[Fact]
        public async Task GetMoviesWithCheapestPriceAsync_HandlesInvalidPriceData()
        {
            // Arrange
            var cinemaWorldMovies = new MovieResponse { Movies = new List<Movie> { new Movie { ID = "cw1", Title = "Movie 1" } } };
            var filmWorldMovies = new MovieResponse { Movies = new List<Movie> { new Movie { ID = "fw1", Title = "Movie 1" } } };
            var movieDetailsWithInvalidPrice = new MovieDetails { Price = "invalid-price" };
            var movieDetailsWithValidPrice = new MovieDetails { Price = "12.50" };

            SetupHttpResponse("http://testapi/cinemaworld/movies", cinemaWorldMovies);
            SetupHttpResponse("http://testapi/filmworld/movies", filmWorldMovies);
            SetupHttpResponse("http://testapi/cinemaworld/movie/cw1", movieDetailsWithInvalidPrice);
            SetupHttpResponse("http://testapi/filmworld/movie/fw1", movieDetailsWithValidPrice);

            // Act
            var result = await _movieService.GetMoviesWithCheapestPriceAsync();

            // Assert
            Assert.Single(result);
            Assert.Equal("Movie 1", result[0].movie.Title);
            Assert.Equal(12.50M, result[0].lowestPrice);
        }

        [Fact]
        public async Task GetAllMovies_ReturnsCombinedListOfMovies()
        {
            // Arrange
            var cinemaWorldMovies = new MovieResponse { Movies = new List<Movie> { new Movie { ID = "cw1", Title = "Movie 1" } } };
            var filmWorldMovies = new MovieResponse { Movies = new List<Movie> { new Movie { ID = "fw1", Title = "Movie 2" } } };

            SetupHttpResponse("http://testapi/cinemaworld/movies", cinemaWorldMovies);
            SetupHttpResponse("http://testapi/filmworld/movies", filmWorldMovies);

            // Act
            var result = await _movieService.GetAllMovies();

            // Assert
            Assert.Equal(2, result.Count);
            Assert.Contains(result, m => m.ID == "cw1");
            Assert.Contains(result, m => m.ID == "fw1");
        }

        [Fact]
        public async Task GetAllMovies_HandlesOneProviderFailure()
        {
            // Arrange
            var filmWorldMovies = new MovieResponse { Movies = new List<Movie> { new Movie { ID = "fw1", Title = "Movie 2" } } };

            SetupHttpResponse<MovieResponse>("http://testapi/cinemaworld/movies", (MovieResponse?)null, HttpStatusCode.InternalServerError);
            SetupHttpResponse("http://testapi/filmworld/movies", filmWorldMovies);

            // Act
            var result = await _movieService.GetAllMovies();

            // Assert
            Assert.Single(result);
            Assert.Equal("Movie 2", result[0].Title);
        }

        [Fact]
        public async Task GetMoviesWithCheapestPriceAsync_HandlesMovieDetailsFailure()
        {
            // Arrange
            var movies = new MovieResponse { Movies = new List<Movie> { new Movie { ID = "cw1", Title = "Movie 1" } } };
            SetupHttpResponse("http://testapi/cinemaworld/movies", movies);
            SetupHttpResponse("http://testapi/filmworld/movies", new MovieResponse { Movies = new List<Movie>() });
            SetupHttpResponse<MovieDetails>("http://testapi/cinemaworld/movie/cw1", (MovieDetails?)null, HttpStatusCode.NotFound);

            // Act
            var result = await _movieService.GetMoviesWithCheapestPriceAsync();

            // Assert
            Assert.Single(result);
            Assert.Equal("Movie 1", result[0].movie.Title);
            Assert.Null(result[0].lowestPrice);
        }
        // ... [rest of your test methods] ...

        private void SetupHttpResponse<T>(string url, T? response, HttpStatusCode statusCode = HttpStatusCode.OK)
        {
            var responseMessage = new HttpResponseMessage
            {
                StatusCode = statusCode,
                Content = response != null ? new StringContent(JsonSerializer.Serialize(response)) : null
            };

            _mockHttpMessageHandler.SetupSendAsync(url, responseMessage);
        }
    }

    public static class MockHttpMessageHandlerExtensions
    {
        public static void SetupSendAsync(this Mock<HttpMessageHandler> handlerMock, string requestUrl, HttpResponseMessage responseMessage)
        {
            handlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.Is<HttpRequestMessage>(req =>
                        req.Method == HttpMethod.Get &&
                        req.RequestUri!.ToString() == requestUrl),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(responseMessage);
        }
    }
}