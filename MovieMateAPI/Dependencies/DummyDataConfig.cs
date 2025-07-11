using MovieMateAPI.Repository;

namespace MovieMateAPI.Dependencies
{
    public static class DummyDataConfig
    {
        public static void AddDummyDataServices(this WebApplicationBuilder builder)
        {
            //Load the data from the Json file
            builder.Services.AddTransient<CinemaWorldData>();
            builder.Services.AddTransient<FilmWorldData>();
            builder.Services.AddTransient<CinemaWorldDetailsData>();
            builder.Services.AddTransient<FilmWorldDetailsData>();

        }
    }
}
