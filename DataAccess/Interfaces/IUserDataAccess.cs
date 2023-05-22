using DataAccess.Models;

namespace DataAccess.Interfaces;
public interface IUserDataAccess
{
    Task<User> CreateUserAsync(User user);
    Task<User> LoginUserAsync(string name);
    Task<User> GetByIdAsync(int id);
    Task SeeMovieAsync(int userId, int movieId);
    Task LikeMovieAsync(int userId, int movieId);
}
