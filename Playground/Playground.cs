using Neo4j.Driver;

namespace Playground;
public class Neo4jMovieService
{
    private readonly IDriver _driver;

    public Neo4jMovieService(string uri, string user, string password)
    {
        _driver = GraphDatabase.Driver(uri, AuthTokens.Basic(user, password));
    }

    public async Task<List<Movie>> GetAllMovies()
    {
        await using var session = _driver.AsyncSession();

        var readResults = await session.ExecuteReadAsync(
            async tx =>
            {
                var movies = new List<Movie>();

                var reader = await tx.RunAsync(
                    "MATCH (m:Movie) RETURN m"
                );

                return await reader.ToListAsync();
            }
        );

        var movies = new List<Movie>();
        foreach(var result in readResults)
        {
            var movieNode = result["m"].As<INode>();
            string movieName = movieNode["name"].As<string>();
            movies.Add(new Movie()
            {
                Name = movieName,
                RelatedMovies = await GetRelatedMovies(session, movieName)
            });
        }

        return movies;
    }

    private async Task<List<Movie>> GetRelatedMovies(IAsyncSession session, string movieName)
    {
        var readResults = await session.ExecuteReadAsync(async tx => {
            var reader = await tx.RunAsync(
                @"MATCH (m:Movie {name: $name})-[:RELATED]->(related:Movie) RETURN related",
                new { name = movieName }
            );

            return await reader.ToListAsync();
        });

        var relatedMovies = new List<Movie>();
        foreach (var result in readResults)
        {
            var relatedMovieNode = result["related"].As<INode>();
            var relatedMovie = new Movie()
            {
                Name = relatedMovieNode["name"].As<string>(),
                RelatedMovies = new List<Movie>()
            };

            relatedMovies.Add(relatedMovie);
        }

        return relatedMovies;
    }

    public void Close()
    {
        _driver?.Dispose();
    }
}