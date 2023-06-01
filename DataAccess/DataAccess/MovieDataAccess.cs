using DataAccess.Interfaces;
using DataAccess.Models;
using Neo4j.Driver;

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
