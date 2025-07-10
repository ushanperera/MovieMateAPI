using Microsoft.AspNetCore.Builder;
using MovieMateAPI.Models;
using MovieMateAPI.Repository;

namespace MovieMateAPI.Endpoints
{
    public static class ApiEnpoints
    {
        public static void UseAppEndpoints(this WebApplication app)
        {
            //app.MapGet("/hello", () => "Hello World!");

            app.MapGet("/", () => "Hellow...");

            //app.MapGet("/samples", (LoadAllSample));

            //app.MapPost("/movies", (Movie movie) =>
            //{
            //    return Results.Ok(movie.Title);
            //});

            //app.MapGet("/cinemaworld/movies", (GetCinemaWorldMovie));
            //app.MapGet("/filmworld/movies", (GetFilmWorldMovie));

            app.MapGet("/cinemaworld/movies", async (HttpContext context) =>
            {
                var data = context.RequestServices.GetService<CinemaWorldData>();
                await context.Response.WriteAsJsonAsync(data?.movieResponse);
            });

            app.MapGet("/filmworld/movies", async (HttpContext context) =>
            {
                var data = context.RequestServices.GetService<FilmWorldData>();
                await context.Response.WriteAsJsonAsync(data?.movieResponse);
            });



            app.MapGet("/cinemaworld/movie/{id}", async (HttpContext context, string id) =>
            {
                var data = context.RequestServices.GetService<CinemaWorldDetailsData>();
                var movie = data?.GetMovieById(id);
                await context.Response.WriteAsJsonAsync(movie);
            });

            app.MapGet("/filmworld/movie/{id}", async (HttpContext context, string id) =>
            {
                var data = context.RequestServices.GetService<FilmWorldDetailsData>();
                var movie = data?.GetMovieById(id);
                await context.Response.WriteAsJsonAsync(movie);
            });



            //app.MapGet("/cinemaworld/movie/{id}", (GetCinemaWorldMovieById));

            //app.MapGet("/filmworld/movie/{id}", (GetFilmWorldMovieById));



            //app.MapGet("/filmworld/movies", GetFilmWorldMovie);
            //app.MapGet("/cinemaworld/movie/{id}", (GetCinemaWorldMovieById));
            //app.MapGet("/filmworld/movie/{id}", (GetFilmWorldMovieById));


        }




        private static IResult GetCinemaWorldMovie(CinemaWorldData data)
        {
            return Results.Ok(data?.movieResponse);
        }

        private static IResult GetFilmWorldMovie(FilmWorldData data)
        {
            return Results.Ok(data?.movieResponse);
        }

        private static async Task<IResult> GetCinemaWorldMovieById(CinemaWorldDetailsData data, int id)
        {
            return Results.Ok("");
        }

        private static async Task<IResult> GetFilmWorldMovieById(FilmWorldDetailsData data, int id)
        {
            return Results.Ok("");
        }



        //private static IResult LoadAllSample(SampleData data)
        //{
        //    return Results.Ok(data?.sampleData);
        //}

        //private static IResult GetCinemaWorldMovie(CinemaWorldData data)
        //{
        //    //try
        //    //{
        //    //var users = await data;
        //    return Results.Ok(data?.movieResponse);
        //    //}
        //    //catch (Exception ex)
        //    //{
        //    //    return Results.Problem(ex.Message);
        //    //}
        //}
        private static async Task<IResult> GetFilmWorldMovie(Movie data)
        {
            return Results.Ok("");
        }
    
        private static async Task<IResult> GetFilmWorldMovieById(Movie data, int id)
        {
            return Results.Ok("");
        }
    }
}
