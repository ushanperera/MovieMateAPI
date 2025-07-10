using MovieMateAPI.Dependencies.Configs;
using MovieMateAPI.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http;
using Xunit;

namespace MovieMateAPI.Tests
{
    public class WebjetApiConfigTests
    {
        [Fact]
        public void Registers_MovieService_And_HttpClient()
        {
            // Arrange
            var config = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string?>
                {{ "WebjetApi:BaseUrl", "https://webjetapitest.azurewebsites.net/" }})
                .Build();

            var builder = WebApplication.CreateBuilder();
            builder.Configuration.AddConfiguration(config);

            // Act
            builder.AddWebjetApiServices();
            var services = builder.Services.BuildServiceProvider();

            // Assert
            Assert.NotNull(services.GetService<IMovieService>());
            var client = services.GetRequiredService<IHttpClientFactory>().CreateClient("WebjetClient");
            Assert.NotNull(client);
            Assert.Equal("https://webjetapitest.azurewebsites.net/", client.BaseAddress?.ToString());
        }

        [Fact]
        public void Throws_When_BaseUrl_Is_Missing()
        {
            var builder = WebApplication.CreateBuilder();
            builder.Configuration.Sources.Clear();

            var ex = Assert.Throws<InvalidOperationException>(() =>
                builder.AddWebjetApiServices());

            Assert.Equal("WebjetApi:BaseUrl is not configured.", ex.Message);
        }
    }
}
