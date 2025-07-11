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

        public async Task<List<(Movie movie, decimal? lowestPrice)>> GetLowestPriceMoviesPriceAsync()
        {
            var allMovies = await GetAllMovies(); //Merged movies from CW & FW
            var results = new List<(Movie, decimal?)>();

            //var groupedMovies = allMovies.GroupBy(m => m.Title); 
            var groupedMovies = allMovies.GroupBy(m => new { m.Title, m.Year }); // Create a unique list
           

            foreach (var group in groupedMovies) // Find the lowest price
            {
                var prices = new List<decimal>();
                var movie = group.First();

                var cwMovie = group.FirstOrDefault(m => m.ID.StartsWith("cw"));
                var fwMovie = group.FirstOrDefault(m => m.ID.StartsWith("fw"));

                if (cwMovie != null) //add CinemaWorld price update
                {
                    var details = await TryGetMovieDetails(Provider.CinemaWorld, cwMovie.ID);
                    if (details != null && Decimal.TryParse(details.Price, out var price))
                    {
                        prices.Add(price);
                    }
                }

                if (fwMovie != null) //add FilmWorld price 
                {
                    var details = await TryGetMovieDetails(Provider.FilmWorld, fwMovie.ID);
                    if (details != null && Decimal.TryParse(details.Price, out var price))
                    {
                        prices.Add(price);
                    }
                }

                decimal? lowest = prices.Any() ? prices.Min() : null;// price comparison here
                results.Add((movie, lowest));
            }

            return results;
        }

        /// <summary>
        /// Merge method. there is a await to makesure both APIs are triggerred
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// this will triger for both movie endpoint
        /// </summary>
        /// <param name="provider"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Thiswill trigger for both movie details endpoints
        /// </summary>
        /// <param name="provider"></param>
        /// <param name="id"></param>
        /// <returns></returns>
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
