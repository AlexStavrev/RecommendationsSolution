using DataAccess.DataAccess;
using Neo4j.Driver;

namespace DataAccess;
public static class DataAccessFactory
{
    public static T GetDataAccess<T>(IDriver driver) where T : class
    {
        return typeof(T).Name switch
        {
            "IMovieDataAccess" => (new MovieDataAccess(driver) as T)!,
            "IUserDataAccess" => (new UserDataAccess(driver) as T)!,
            _ => throw new ArgumentException($"Unknown type {typeof(T).FullName}"),
        };
    }
}
