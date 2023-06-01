using DataAccessSQL.Interfaces;
using DataAccessSQL.Models;
using Dapper;
using System.Data;

namespace DataAccessSQL.DataAccess;
internal class UserDataAccessSQL : IUserDataAccessSQL
{
    private readonly IDbConnection _con;

    public UserDataAccessSQL(IDbConnection con)
    {
        _con = con;
    }

    public async Task<int?> CreateUserAsync(UserSQL user)
    {
        var query = "INSERT INTO [User] ([Name]) VALUES (@name); SELECT SCOPE_IDENTITY()";
        var parameters = new { name = user.name };
        var userId = await _con.ExecuteScalarAsync<int?>(query, parameters);

        return userId;
    }

    public async Task<UserSQL> GetByIdAsync(int id)
    {
        var query = "SELECT * FROM [User] WHERE Id = @id";
        var parameters = new { id };
        var user = await _con.QueryFirstOrDefaultAsync<UserSQL>(query, parameters);

        return user;
    }

    public async Task<IEnumerable<MovieSQL>> GetRecomendationsAsync(int userId)
    {
        var parameters = new { userId };
        var movies = await _con.QueryAsync<MovieSQL>("GetRelatedMovies", parameters,
            commandType: CommandType.StoredProcedure);

        return movies;
    }
}
