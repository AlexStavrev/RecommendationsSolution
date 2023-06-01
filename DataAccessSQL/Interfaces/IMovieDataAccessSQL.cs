using DataAccessSQL.Models;

namespace DataAccessSQL.Interfaces;
public interface IMovieDataAccessSQL
{
    Task<IEnumerable<MovieSQL>> GetAllAsync();
    Task<MovieSQL?> GetByIdAsync(int id);
}
