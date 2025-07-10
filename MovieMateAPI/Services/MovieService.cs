using MovieMateAPI.Models;


namespace MovieMateAPI.Services
{
    public class MovieService : IMovieService
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl;

        public MovieService(IConfiguration config, IHttpClientFactory factory)
        {
            _httpClient = factory.CreateClient("WebjetClient");
            _baseUrl = config["WebjetApi:BaseUrl"]!;
            _httpClient.DefaultRequestHeaders.Add("x-access-token", config["WebjetApi:ApiToken"]);
        }

        public async Task<List<(Movie movie, decimal? lowestPrice)>> GetMoviesWithCheapestPriceAsync()
        {
            var allMovies = await GetAllMovies();

            var results = new List<(Movie, decimal?)>();

            foreach (var movie in allMovies.DistinctBy(m => m.Title))
            {
                var prices = new List<decimal>();

                var cw = await TryGetMovieDetails("cinemaworld", movie.ID);
                if (cw?.Price != null) prices.Add(Decimal.Parse(cw.Price));

                var fw = await TryGetMovieDetails("filmworld", movie.ID);
                if (fw?.Price != null) prices.Add(Decimal.Parse(fw.Price));

                decimal? lowest = prices.Any() ? prices.Min() : null;
                results.Add((movie, lowest));
            }

            return results;
        }

        private async Task<List<Movie>> GetAllMovies()
        {
            var cwTask = TryGetMovies("cinemaworld");
            var fwTask = TryGetMovies("filmworld");

            await Task.WhenAll(cwTask, fwTask);

            var combined = new List<Movie>();
            if (cwTask.Result != null) combined.AddRange(cwTask.Result);
            if (fwTask.Result != null) combined.AddRange(fwTask.Result);

            return combined;
        }

        private async Task<List<Movie>?> TryGetMovies(string provider)
        {
            try
            {
                var response = await _httpClient.GetFromJsonAsync<MovieResponse>($"{_baseUrl}{provider}/movies");
                return response?.Movies;
            }
            catch
            {
                return null;
            }
        }

        private async Task<MovieDetails?> TryGetMovieDetails(string provider, string id)
        {
            try
            {
                return await _httpClient.GetFromJsonAsync<MovieDetails>($"{_baseUrl}{provider}/movie/{id}");
            }
            catch
            {
                return null;
            }
        }
    }

}
