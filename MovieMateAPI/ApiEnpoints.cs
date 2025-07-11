using MovieMateAPI.Repository;
using MovieMateAPI.Services;

namespace MovieMateAPI.Endpoints
{
    public static class ApiEndpoints
    {
        public static void UseAppEndpoints(this WebApplication app)
        {

            app.MapGet("/api/movies/compare", GetLowestPriceMovies);

            app.MapGet("/api/cinemaworld/movies", GetCinemaWorldMovie);
            app.MapGet("/api/filmworld/movies", GetFilmWorldMovie);
            app.MapGet("/api/cinemaworld/movie/{id}", GetCinemaWorldMovieById);
            app.MapGet("/api/filmworld/movie/{id}", GetFilmWorldMovieById);

        }
        public static async Task<IResult> GetLowestPriceMovies(IMovieService service, ILoggerFactory loggerFactory)
        {
            var data = await service.GetLowestPriceMoviesPriceAsync();

            if (data == null)
            {
                var logger = loggerFactory.CreateLogger("Compare Movies");
                logger.LogWarning("CheaperMoviePrice Response is null.");

                return Results.NotFound("No data found.");
            }
            return Results.Ok(data.Select(x => new
            {
                x.movie.Title,
                x.movie.Year,
                x.movie.Poster,
                CheapestPrice = x.lowestPrice.HasValue ? x.lowestPrice.Value.ToString() : "Unavailable"
            }));
        }

        public static IResult GetCinemaWorldMovie(CinemaWorldData data, ILoggerFactory loggerFactory)
        {
            try
            {
                if (data?.movieResponse == null || data.movieResponse.Movies == null)
                {
                    var logger = loggerFactory.CreateLogger("CinemaWorld");
                    logger.LogWarning("movieResponse was null");

                    return Results.NotFound("No data found.");
                }

                return Results.Ok(data.movieResponse);
            }
            catch (Exception ex)
            {
                var logger = loggerFactory.CreateLogger("CinemaWorld");
                logger.LogError(ex, "Unhandled error in GetCinemaWorldMovie");

                return Results.Problem("An error occurred while retrieving data.");
            }
        }

        private static IResult GetFilmWorldMovie(FilmWorldData data, ILoggerFactory loggerFactory)
        {
            if (data?.movieResponse == null)
            {
                var logger = loggerFactory.CreateLogger("FilmWorld");
                logger.LogWarning("FilmWorld movieResponse is null.");

                return Results.NotFound("No data found.");
            }

            return Results.Ok(data.movieResponse);
        }

        private static IResult GetCinemaWorldMovieById(CinemaWorldDetailsData data, string id, ILoggerFactory loggerFactory)
        {
            if (data == null)
            {
                var logger = loggerFactory.CreateLogger("CinemaWorld");
                logger.LogWarning("CinemaWorldDetailsData is null for ID {Id}", id);

                return Results.NotFound("No data found.");
            }

            var movie = data.GetMovieById(id);

            if (movie == null)
            {
                var logger = loggerFactory.CreateLogger("CinemaWorld");
                logger.LogWarning("No movie found with ID {Id} in CinemaWorld.", id);

                return Results.NotFound($"No movie found with ID {id}.");
            }

            return Results.Ok(movie);
        }

        private static IResult GetFilmWorldMovieById(FilmWorldDetailsData data, string id, ILoggerFactory loggerFactory)
        {
            if (data == null)
            {
                var logger = loggerFactory.CreateLogger("FilmWorld");
                logger.LogWarning("FilmWorldDetailsData is null for ID {Id}", id);

                return Results.NotFound("No data found.");
            }

            var movie = data.GetMovieById(id);

            if (movie == null)
            {
                var logger = loggerFactory.CreateLogger("FilmWorld");
                logger.LogWarning("No movie found with ID {Id} in FilmWorld.", id);

                return Results.NotFound($"No movie found with ID {id}.");
            }

            return Results.Ok(movie);
        }
    }
}
