using DataAccess;
using DataAccess.Interfaces;
using Neo4j.Driver;

var builder = WebApplication.CreateBuilder(args);

//Create Neo4J driver
string user = "neo4j";
string? password = builder.Configuration["Neo4jPassword"];
var _driver = GraphDatabase.Driver("bolt://localhost:7687", AuthTokens.Basic(user, password));

// Add services to the container.
builder.Services.AddScoped(u => DataAccessFactory.GetDataAccess<IMovieDataAccess>(_driver));
builder.Services.AddScoped(m => DataAccessFactory.GetDataAccess<IUserDataAccess>(_driver));
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
