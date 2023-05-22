using DataAccess.Interfaces;
using DataAccess.Models;

namespace DataAccess.DataAccess;
internal class MovieDataAccess : IMovieDataAccess
{
    public Task<IEnumerable<Movie>> GetAllAsync()
    {
        throw new NotImplementedException();
    }

    public Task<Movie> GetByIdAsync(int id)
    {
        throw new NotImplementedException();
    }
}
