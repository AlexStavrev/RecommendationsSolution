using DataAccess.Interfaces;
using DataAccess.Models;

namespace DataAccess.DataAccess;
internal class UserDataAccess : IUserDataAccess
{
    public Task<User> CreateUserAsync(User user)
    {
        throw new NotImplementedException();
    }

    public Task<User> GetByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task LikeMovieAsync(int userId, int movieId)
    {
        throw new NotImplementedException();
    }

    public Task<User> LoginUserAsync(string name)
    {
        throw new NotImplementedException();
    }

    public Task SeeMovieAsync(int userId, int movieId)
    {
        throw new NotImplementedException();
    }
}
