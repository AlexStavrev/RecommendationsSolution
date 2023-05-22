using DataAccess.Models;

namespace DataAccess.Interfaces;
public interface IMovieDataAccess
{
    Task<IEnumerable<Movie>> GetAllAsync();
    Task<Movie?> GetByIdAsync(int id);
}
