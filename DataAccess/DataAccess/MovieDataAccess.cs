using DataAccess.Interfaces;
using DataAccess.Models;
using Neo4j.Driver;

namespace DataAccess.DataAccess;
internal class MovieDataAccess : IMovieDataAccess
{
    private readonly IDriver _driver;
    public MovieDataAccess(IDriver driver)
    {
        _driver = driver;
    }

    public async Task<IEnumerable<Movie>> GetAllAsync()
    {
        await using var session = _driver.AsyncSession();

        var readResults = await ExecuteQueryAsync("MATCH (m:Movie) RETURN m");

        var movies = new List<Movie>();
        foreach (var result in readResults)
        {
            var movieNode = result["m"].As<INode>();
            movies.Add(new Movie()
            {
                Name = movieNode["name"].As<string>(),
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
            Name = movieNode["name"].As<string>(),
        };

        return movie;
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
