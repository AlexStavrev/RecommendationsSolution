namespace DataAccess.Models;
public record Movie
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Url { get; set; }
    public bool? Seen { get; set; }
    public bool? Liked { get; set; }
}
