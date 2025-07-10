namespace MovieMateAPI
{
    public static class ApiEnpoints
    {
        public static void UseAppEndpoints(this WebApplication app)
        {
            app.MapGet("/cinemaworld/movies", GetCinemaWorldMovie);
            app.MapGet("/filmworld/movies", GetFilmWorldMovie);
            app.MapGet("/cinemaworld/movie/{id}", (GetCinemaWorldMovieById));
            app.MapGet("/filmworld/movie/{id}", (GetFilmWorldMovieById));

  
        }

        private static async Task<IResult> GetCinemaWorldMovie(Models.Movie data)
        {
            //try
            //{
            //var users = await data;
            return Results.Ok("");
            //}
            //catch (Exception ex)
            //{
            //    return Results.Problem(ex.Message);
            //}
        }
        private static async Task<IResult> GetFilmWorldMovie(Models.Movie data)
        {
            return Results.Ok("");
        }
        private static async Task<IResult> GetCinemaWorldMovieById(Models.Movie data, int id)
        {
            return Results.Ok("");
        }
        private static async Task<IResult> GetFilmWorldMovieById(Models.Movie data, int id)
        {
            return Results.Ok("");
        }
    }
}
