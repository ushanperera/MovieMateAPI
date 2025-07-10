using MovieMateAPI.Repository;
using System.ComponentModel.DataAnnotations;

namespace MovieMateAPI.Models
{
    public class Movie
    {
        [Required]
        public string ID { get; set; } = string.Empty;

        [Required]
        [MinLength(2)]
        public string Title { get; set; } = string.Empty;
        public string? Year { get; set; }
        public string? Type { get; set; }
        public string? Poster { get; set; }
    }
}
