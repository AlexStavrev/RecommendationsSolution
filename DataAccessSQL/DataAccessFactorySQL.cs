using DataAccessSQL.DataAccess;
using System.Data;

namespace DataAccessSQL;
public static class DataAccessFactorySQL
{
    public static T GetDataAccess<T>(IDbConnection con) where T : class
    {
        return typeof(T).Name switch
        {
            "IMovieDataAccessSQL" => (new MovieDataAccessSQL(con) as T)!,
            "IUserDataAccessSQL" => (new UserDataAccessSQL(con) as T)!,
            _ => throw new ArgumentException($"Unknown type {typeof(T).FullName}"),
        };
    }
}
