using DataAccess.Interfaces;
using DataAccess.Models;
using Neo4j.Driver;

namespace DataAccess.DataAccess;
internal class UserDataAccess : IUserDataAccess
{
    private readonly IDriver _driver;
    private IAsyncSession _session;

    public UserDataAccess(IDriver driver)
    {
        _driver = driver;
        Task.Run(() => _session = _driver.AsyncSession());
    }

    public async Task<int?> CreateUserAsync(User user)
    {
        var readResults = await ExecuteWriteQueryAsync("CREATE (u:User {name: $name}) RETURN ID(u)", new { name = user.Name });

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
        var readResults = await ExecuteReadQueryAsync("MATCH (u:User) WHERE ID(u) = $id RETURN u", new { id });

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

        _ = await ExecuteWriteQueryAsync(query, new { userId, movieId, likeWeight });
    }

    public async Task<User> LoginUserAsync(string name)
    {
        var readResults = await ExecuteReadQueryAsync("MATCH (u:User) WHERE u.name = $name RETURN u", new { name });

        var result = readResults.FirstOrDefault();
        if (result == null)
        {
            return null; // User not found
        }

        var userNode = result["u"].As<INode>();
        var user = new User()
        {
            Id = userNode.Id.As<int>(),
            Name = userNode["name"].As<string>(),
        };

        return user;
    }

    public async Task SeeMovieAsync(int userId, int movieId)
    {
        float seeWeight = 0.1f;
        string query = @"
            MATCH (u:User) WHERE ID(u) = $userId
            MATCH (m:Movie) WHERE ID(m) = $movieId
            MERGE (u)-[r:INTERACTED]->(m)
            ON CREATE SET r.weight = $seeWeight
            ON MATCH SET r.weight = CASE WHEN r.weight < $seeWeight THEN r.weight + $seeWeight ELSE r.weight END";

        _ = await ExecuteWriteQueryAsync(query, new { userId, movieId, seeWeight });
    }

    public async Task<IEnumerable<Movie>> GetRecomendationsAsync(int userId)
    {
        string query = @"
            MATCH (u:User)-[i:INTERACTED]->(movie:Movie)-[r:RELATED]->(relatedMovie:Movie)
            WHERE ID(u) = $userId
            WITH relatedMovie, COLLECT(TOFLOAT(i.weight))
            AS interactedWeights, COLLECT(TOFLOAT(r.weight))
            AS relatedWeights
            RETURN relatedMovie,
            REDUCE(total = 0.0, weight IN interactedWeights + relatedWeights | total + weight)
            AS totalWeight
            ORDER BY totalWeight DESC";

        var readResults = await ExecuteReadQueryAsync(query, new {userId});

        var movies = new List<Movie>();
        foreach (var result in readResults)
        {
            var movieNode = result["relatedMovie"].As<INode>();
            movies.Add(new Movie()
            {
                Id = movieNode.Id.As<int>(),
                Name = movieNode["name"].As<string>(),
                Url = movieNode["url"].As<string>()
            });
        }
        return movies;
    }

    private async Task<IEnumerable<IRecord>> ExecuteReadQueryAsync(string cypherQuery, object parameters = null)
    {
        return await _session.ExecuteReadAsync(async queryRunner =>
        {
            var reader = await queryRunner.RunAsync(cypherQuery, parameters);
            return await reader.ToListAsync();
        });
    }

    private async Task<IEnumerable<IRecord>> ExecuteWriteQueryAsync(string cypherQuery, object parameters = null)
    {
        return await _session.ExecuteWriteAsync(async queryRunner =>
        {
            var reader = await queryRunner.RunAsync(cypherQuery, parameters);
            return await reader.ToListAsync();
        });
    }
}
