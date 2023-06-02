using DataAccess.Interfaces;
using DataAccess.Models;
using Neo4j.Driver;
using System.Xml.Linq;

namespace DataAccess.DataAccess;
internal class MovieDataAccess : IMovieDataAccess
{
    private readonly IDriver _driver;
    private IAsyncSession _session;
    public MovieDataAccess(IDriver driver)
    {
        _driver = driver;
        Task.Run(() => _session = _driver.AsyncSession());
    }

    public async Task<IEnumerable<Movie>> GetAllAsync()
    {
        var readResults = await ExecuteQueryAsync("MATCH (m:Movie) RETURN m");

        var movies = new List<Movie>();
        foreach (var result in readResults)
        {
            var movieNode = result["m"].As<INode>();
            movies.Add(new Movie()
            {
                Id = movieNode.Id.As<int>(),
                Name = movieNode["name"].As<string>(),
                Url = movieNode["url"].As<string>(),
            });
        }
        return movies;
    }

    public async Task<IEnumerable<Movie>> GetAllByUserIdAsync(int userId)
    {
        await using var session = _driver.AsyncSession();

        var readResults = await ExecuteQueryAsync(@"
            MATCH(m: Movie)
            OPTIONAL MATCH(m) < -[r: INTERACTED] - (u: User)
            WHERE ID(u) = $userId
            RETURN m, 
            CASE WHEN r IS NULL THEN false ELSE true END AS seen, 
            CASE WHEN r IS NULL THEN false ELSE r.weight > 0.9 END AS liked", new { userId });

        var movies = new List<Movie>();
        foreach (var result in readResults)
        {
            var movieNode = result["m"].As<INode>();
            var seen = result["seen"].As<bool>();
            var liked = result["liked"].As<bool>();
            movies.Add(new Movie()
            {
                Id = movieNode.Id.As<int>(),
                Name = movieNode["name"].As<string>(),
                Url = movieNode["url"].As<string>(),
                Seen = seen,
                Liked = liked,
            });
        }
        return movies;
    }

    public async Task<Movie?> GetByIdAsync(int id)
    {
        var readResults = await ExecuteQueryAsync("MATCH (m:Movie) WHERE ID(m) = $id RETURN m", new { id });

        var result = readResults.FirstOrDefault();
        if (result == null)
        {
            return null; // Movie not found
        }

        var movieNode = result["m"].As<INode>();
        var movie = new Movie()
        {
            Id = movieNode.Id.As<int>(),
            Name = movieNode["name"].As<string>(),
            Url = movieNode["url"].As<string>(),
        };

        return movie;
    }

    private async Task<IEnumerable<IRecord>> ExecuteQueryAsync(string cypherQuery, object parameters = null)
    {
        return await _session.ExecuteReadAsync(async queryRunner =>
        {
            var reader = await queryRunner.RunAsync(cypherQuery, parameters);
            return await reader.ToListAsync();
        });
    }
}
