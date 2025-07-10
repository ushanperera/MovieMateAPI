using Microsoft.Extensions.Logging;
using MovieMateAPI.Repository;

namespace MovieMateAPI.Endpoints
{
    public static class ApiEndpoints
    {
        public static void UseAppEndpoints(this WebApplication app)
        {
            app.MapGet("/cinemaworld/movies", GetCinemaWorldMovie);
            app.MapGet("/filmworld/movies", GetFilmWorldMovie);

            app.MapGet("/cinemaworld/movie/{id}", GetCinemaWorldMovieById);
            app.MapGet("/filmworld/movie/{id}", GetFilmWorldMovieById);
        }

        private static IResult GetCinemaWorldMovie(CinemaWorldData data, ILoggerFactory loggerFactory)
        {
            try
            {
                if (data?.movieResponse == null)
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
