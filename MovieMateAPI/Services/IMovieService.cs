using MovieMateAPI.Models;

namespace MovieMateAPI.Services
{
    public interface IMovieService
    {
        Task<List<(Movie movie, decimal? lowestPrice)>> GetMoviesWithCheapestPriceAsync();
    }
}
