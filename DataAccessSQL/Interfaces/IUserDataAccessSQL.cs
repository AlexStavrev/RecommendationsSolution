using DataAccessSQL.Models;

namespace DataAccessSQL.Interfaces;
public interface IUserDataAccessSQL
{
    Task<int?> CreateUserAsync(UserSQL user);
    Task<UserSQL> GetByIdAsync(int id);
    Task<IEnumerable<MovieSQL>> GetRecomendationsAsync(int userId);
}
