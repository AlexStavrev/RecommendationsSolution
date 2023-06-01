// See https://aka.ms/new-console-template for more information
using BenchmarkDotNet.Running;
using Playground;

//Console.WriteLine("Hello, World!");

//var movieService = new Neo4jMovieService("bolt://localhost:7687", "neo4j", "password");
//var movies = await movieService.GetAllMovies();
//movies.ForEach(Console.WriteLine);
//movieService.Close();

var summary = BenchmarkRunner.Run<Benchmarks>();