using MovieMateAPI.Models;
using System.Text.Json;

namespace MovieMateAPI.Repository
{
    public class FilmWorldDetailsData
    {
        public List<MovieDetails> movieDetailList { get; private set; }
        public FilmWorldDetailsData()
        {
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            string jsonFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Data", "filmWorldDetails.json");

            string jsonString = File.ReadAllText(jsonFilePath);

            movieDetailList = JsonSerializer.Deserialize<List<MovieDetails>>(jsonString, options) ?? new();

        }

        public MovieDetails? GetMovieById(string movieId)
        {
            return movieDetailList.FirstOrDefault(movie => movie.ID == movieId);
        }
    }
}
