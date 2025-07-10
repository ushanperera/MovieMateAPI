using MovieMateAPI.Constant;
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

        public async Task<List<(Movie movie, decimal? lowestPrice)>> GetCheaperMoviePriceAsync()
        {
            var allMovies = await GetAllMovies();

            var results = new List<(Movie, decimal?)>();

            var groupedMovies = allMovies.GroupBy(m => m.Title);

            foreach (var group in groupedMovies)
            {
                var prices = new List<decimal>();
                var movie = group.First(); // Use the first movie as the representative for the group

                var cwMovie = group.FirstOrDefault(m => m.ID.StartsWith("cw"));
                var fwMovie = group.FirstOrDefault(m => m.ID.StartsWith("fw"));

                if (cwMovie != null)
                {
                    var details = await TryGetMovieDetails(Provider.CinemaWorld, cwMovie.ID);
                    if (details != null && Decimal.TryParse(details.Price, out var price))
                    {
                        prices.Add(price);
                    }
                }

                if (fwMovie != null)
                {
                    var details = await TryGetMovieDetails(Provider.FilmWorld, fwMovie.ID);
                    if (details != null && Decimal.TryParse(details.Price, out var price))
                    {
                        prices.Add(price);
                    }
                }

                decimal? lowest = prices.Any() ? prices.Min() : null;
                results.Add((movie, lowest));
            }

            return results;
        }

        public async Task<List<Movie>> GetAllMovies()
        {
            var cwTask = TryGetMovies(Provider.CinemaWorld);
            var fwTask = TryGetMovies(Provider.FilmWorld);

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
