// See https://aka.ms/new-console-template for more information
using Playground;

Console.WriteLine("Hello, World!");

var movieService = new Neo4jMovieService("bolt://localhost:7687", "neo4j", "password");
var movies = await movieService.GetAllMovies();
movies.ForEach(Console.WriteLine);
movieService.Close();

/* DataAccess

- create user
- login user by name
- get user by id
- get movie by id
- interact with video
- like video
- get recommendations
- get all movies

 */