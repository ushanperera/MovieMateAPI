using MovieMateAPI.Models;
using System.Text.Json;

namespace MovieMateAPI.Repository
{
    public class CinemaWorldDetailsData
    {
        public List<MovieDetails> movieDetailList { get; private set; }

        public CinemaWorldDetailsData()
        {
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            string jsonFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Data", "cinemaWorldDetails.json");
            string jsonString = File.ReadAllText(jsonFilePath);
            movieDetailList = JsonSerializer.Deserialize<List<MovieDetails>>(jsonString, options) ?? new();
        }

        public MovieDetails? GetMovieById(string movieId)
        {
            return movieDetailList.FirstOrDefault(movie => movie.ID == movieId);
        }
    }
}
