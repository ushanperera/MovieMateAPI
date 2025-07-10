using System.ComponentModel.DataAnnotations;

namespace MovieMateAPI.Models
{
    public class MovieResponse
    {
        public List<Movie> Movies { get; set; }
    }
    public class Movie
    {
        [Required]
        public required string ID { get; set; }

        [Required]
        [MinLength(2)]
        public string Title { get; set; }
        public string? Year { get; set; }
        public string? Type { get; set; }
        public string? Poster { get; set; }
    }
}
