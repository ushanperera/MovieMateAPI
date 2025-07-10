using Microsoft.AspNetCore.Builder;
using MovieMateAPI.Models;
using MovieMateAPI.Repository;

namespace MovieMateAPI.Endpoints
{
    public static class ApiEnpoints
    {
        public static void UseAppEndpoints(this WebApplication app)
        {

            app.MapGet("/cinemaworld/movies", (GetCinemaWorldMovie));
            app.MapGet("/filmworld/movies", (GetFilmWorldMovie));

            app.MapGet("/cinemaworld/movie/{id}", (GetCinemaWorldMovieById));
            app.MapGet("/filmworld/movie/{id}", (GetFilmWorldMovieById));


            //app.MapGet("/cinemaworld/movies", async (HttpContext context) =>
            //{
            //    var data = context.RequestServices.GetService<CinemaWorldData>();
            //    await context.Response.WriteAsJsonAsync(data?.movieResponse);
            //});

            //app.MapGet("/filmworld/movies", async (HttpContext context) =>
            //{
            //    var data = context.RequestServices.GetService<FilmWorldData>();
            //    await context.Response.WriteAsJsonAsync(data?.movieResponse);
            //});

            //app.MapGet("/cinemaworld/movie/{id}", async (HttpContext context, string id) =>
            //{
            //    var data = context.RequestServices.GetService<CinemaWorldDetailsData>();
            //    var movie = data?.GetMovieById(id);
            //    await context.Response.WriteAsJsonAsync(movie);
            //});

            //app.MapGet("/filmworld/movie/{id}", async (HttpContext context, string id) =>
            //{
            //    var data = context.RequestServices.GetService<FilmWorldDetailsData>();
            //    var movie = data?.GetMovieById(id);
            //    await context.Response.WriteAsJsonAsync(movie);
            //});

        }

        private static IResult GetCinemaWorldMovie(CinemaWorldData data, ILoggerFactory loggerFactory)
        {
            try
            {
                if (data.movieResponse == null)
                {
                    loggerFactory.CreateLogger("CinemaWorld movieResponse was null");
                    return Results.NotFound("No Data found");
                }
                return Results.Ok(data?.movieResponse);
            }
            catch (Exception ex)
            {
                loggerFactory.CreateLogger("Error in GetCinemaWorldMovie:" + ex.Message);
                return Results.InternalServerError(ex.Message);
            }

        }

        private static IResult GetFilmWorldMovie(FilmWorldData data)
        {
            if (data.movieResponse == null)
            {
                return Results.NotFound("No Data found");
            }
            return Results.Ok(data?.movieResponse);
        }

        private static IResult GetCinemaWorldMovieById(CinemaWorldDetailsData data, string id)
        {
            if (data == null)
            {
                return Results.NotFound("No Data found");
            }
            return Results.Ok(data?.GetMovieById(id));
        }

        private static IResult GetFilmWorldMovieById(FilmWorldDetailsData data, string id)
        {
            if (data == null)
            {
                return Results.NotFound("No Data found");
            }
            return Results.Ok(data?.GetMovieById(id));
        }

    }
}
