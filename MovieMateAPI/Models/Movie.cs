using MovieMateAPI.Repository;
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
        public string ID { get; set; } // Removed 'required' keyword to ensure compatibility with C# 10.0

        [Required]
        [MinLength(2)]
        public string Title { get; set; }
        public string? Year { get; set; }
        public string? Type { get; set; }
        public string? Poster { get; set; }
    }
}
