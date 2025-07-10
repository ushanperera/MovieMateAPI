using MovieMateAPI.Models;
using System.Text.Json;

namespace MovieMateAPI.Repository
{
    public class CinemaWorldData
    {
        public MovieResponse movieResponse { get; private set; }

        public CinemaWorldData()
        {
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            string jsonFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Data", "cinemaWorld.json");
            string jsonString = File.ReadAllText(jsonFilePath);
            List<Movie> movieList = JsonSerializer.Deserialize<List<Movie>>(jsonString, options) ?? new();

            movieResponse = new MovieResponse
            {
                Movies = movieList
            };
        }

    
    }
}
