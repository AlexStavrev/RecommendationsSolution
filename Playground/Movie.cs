using System.Text;

namespace Playground;

public record Movie
{
    public string Name { get; set; }
    public IEnumerable<Movie> RelatedMovies { get; set; }

    public override string ToString()
    {
        var sb = new StringBuilder();
        sb.AppendLine($"Movie: {Name}");

        if (RelatedMovies != null && RelatedMovies.Any())
        {
            sb.AppendLine("Related Movies:");
            foreach (var relatedMovie in RelatedMovies)
            {
                sb.AppendLine($"- {relatedMovie.Name}");
            }
        }
        else
        {
            sb.AppendLine("No related movies found.");
        }

        return sb.ToString();
    }
}