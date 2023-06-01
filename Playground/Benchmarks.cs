using BenchmarkDotNet.Attributes;
using DataAccess;
using DataAccess.Interfaces;
using DataAccess.Models;
using DataAccessSQL;
using DataAccessSQL.Interfaces;
using DataAccessSQL.Models;
using Microsoft.Extensions.Configuration;
using Neo4j.Driver;
using System.Data.SqlClient;

namespace Playground;

[MemoryDiagnoser]
public class Benchmarks
{
    private readonly IMovieDataAccess _movieDataAccess;
    private readonly IMovieDataAccessSQL _movieDataAccessSQL;
    private readonly IUserDataAccess _userDataAccess;
    private readonly IUserDataAccessSQL _userDataAccessSQL;

    public Benchmarks()
	{
        var builder = new ConfigurationBuilder().AddUserSecrets<Program>();
        IConfiguration config = builder.Build();
        var _driver = GraphDatabase.Driver(
            "bolt://localhost:7687",
            AuthTokens.Basic("neo4j", config["Neo4jPassword"])
        );
        _movieDataAccess = DataAccessFactory.GetDataAccess<IMovieDataAccess>(_driver);
        _userDataAccess = DataAccessFactory.GetDataAccess<IUserDataAccess>(_driver);

        var _con = new SqlConnection("Data Source=.;Initial Catalog=MoviesDatabase;Integrated Security=True");
        _movieDataAccessSQL = DataAccessFactorySQL.GetDataAccess<IMovieDataAccessSQL>(_con);
        _userDataAccessSQL = DataAccessFactorySQL.GetDataAccess<IUserDataAccessSQL>(_con);
    }

    [Benchmark]
    public async Task<UserSQL> SQL_GetByIdAsync()
    {
        return await _userDataAccessSQL.GetByIdAsync(1);
    }
    [Benchmark]
    public async Task<User?> Neo4j_GetByIdAsync()
    {
        return await _userDataAccess.GetByIdAsync(2);
    }

    [Benchmark]
    public async Task<List<MovieSQL>> SQL_GetRecomendationsAsync()
    {
        return (await _userDataAccessSQL.GetRecomendationsAsync(1)).ToList();
    }
    [Benchmark]
    public async Task<List<Movie>> Neo4j_GetRecomendationsAsync()
    {
        return (await _userDataAccess.GetRecomendationsAsync(2)).ToList();
    }

    [Benchmark]
    public async Task<List<MovieSQL>> SQL_GetAllAsync()
    {
        return (await _movieDataAccessSQL.GetAllAsync()).ToList();
    }
    [Benchmark]
    public async Task<List<Movie>> Neo4j_GetAllAsync()
    {
        return (await _movieDataAccess.GetAllAsync()).ToList();
    }
}
