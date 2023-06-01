using DataAccessSQL.Interfaces;
using DataAccessSQL.Models;
using System.Data;
using Dapper;

namespace DataAccessSQL.DataAccess;
internal class MovieDataAccessSQL : IMovieDataAccessSQL
{
    private readonly IDbConnection _con;
    public MovieDataAccessSQL(IDbConnection con)
    {
        _con = con;
    }

    public async Task<IEnumerable<MovieSQL>> GetAllAsync()
    {
        var query = "SELECT * FROM Movie";
        var movies = await _con.QueryAsync<MovieSQL>(query);

        return movies;
    }

    public async Task<MovieSQL?> GetByIdAsync(int id)
    {
        var query = "SELECT * FROM Movie WHERE id = @id";
        var movie = await _con.QueryFirstOrDefaultAsync<MovieSQL>(query, new { id });

        return movie;
    }
}
