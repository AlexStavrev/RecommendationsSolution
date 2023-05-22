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

    public async Task<int?> CreateUserAsync(User user)
    {
        var readResults = await ExecuteQueryAsync("CREATE (u:User {name: $name}) RETURN ID(u)", new { name = user.Name });

        var result = readResults.FirstOrDefault();
        if (result == null || !result.Keys.Contains("ID(u)"))
        {
            return null; // User creation failed or ID not found
        }

        var userId = result["ID(u)"].As<int?>();
        return userId;
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
        float likeWeight = 0.9f;
        string query = @"
            MATCH (u:User) WHERE ID(u) = $userId
            MATCH (m:Movie) WHERE ID(m) = $movieId
            MERGE (u)-[r:INTERACTED]->(m)
            ON CREATE SET r.weight = $likeWeight
            ON MATCH SET r.weight = CASE WHEN r.weight < $likeWeight THEN r.weight + $likeWeight ELSE r.weight END";

        _ = await ExecuteQueryAsync(query, new { userId, movieId, likeWeight });
    }

    public async Task<User> LoginUserAsync(string name)
    {
        var readResults = await ExecuteQueryAsync("MATCH (u:User) WHERE u.name = $name RETURN u", new { name });

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
