using DataAccess.Interfaces;
using DataAccess.Models;
using Neo4j.Driver;

namespace DataAccess.DataAccess;
internal class UserDataAccess : IUserDataAccess
{

    private readonly IDriver _driver;

    public UserDataAccess(IDriver driver)
    {
        _driver = driver;
    }

    public async Task<User> CreateUserAsync(User user)
    {
        throw new NotImplementedException();
    }

    public async Task<User> GetByIdAsync(int id)
    {
        var readResults = await ExecuteQueryAsync("MATCH (u:User) WHERE ID(u) = $id RETURN u", new { id });

        var result = readResults.FirstOrDefault();
        if (result == null)
        {
            return null; // User not found
        }

        var userNode = result["u"].As<INode>();
        var user = new User()
        {
            Name = userNode["name"].As<string>(),
        };

        return user;
    }

    public async Task LikeMovieAsync(int userId, int movieId)
    {
        throw new NotImplementedException();
    }

    public async Task<User> LoginUserAsync(string name)
    {
        throw new NotImplementedException();
    }

    public async Task SeeMovieAsync(int userId, int movieId)
    {
        throw new NotImplementedException();
    }

    private async Task<IEnumerable<IRecord>> ExecuteQueryAsync(string cypherQuery, object parameters = null)
    {
        await using var session = _driver.AsyncSession();
        return await session.ExecuteReadAsync(async queryRunner =>
        {
            var reader = await queryRunner.RunAsync(cypherQuery, parameters);
            return await reader.ToListAsync();
        });
    }
}
