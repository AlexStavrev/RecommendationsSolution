using DataAccess.Models;

namespace DataAccess.Interfaces;
public interface IMovieDataAccess
{
    Task<IEnumerable<Movie>> GetAllAsync();
    Task<IEnumerable<Movie>> GetAllByUserIdAsync(int userId);
    Task<Movie?> GetByIdAsync(int id);
}
