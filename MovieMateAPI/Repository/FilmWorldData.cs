using MovieMateAPI.Models;
using System.Text.Json;

namespace MovieMateAPI.Repository
{
    public class FilmWorldData
    {
        public List<Movie> movieList { get; private set; }
        public MovieResponse movieResponse { get; private set; }

        public FilmWorldData()
        {
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            string jsonFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Data", "filmWorld.json");

            string jsonString = File.ReadAllText(jsonFilePath);

            movieList = JsonSerializer.Deserialize<List<Movie>>(jsonString, options) ?? new();

            movieResponse = new MovieResponse
            {
                Movies = movieList
            };
        }
    }
}
