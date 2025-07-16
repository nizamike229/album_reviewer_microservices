namespace AlbumService.Domain.Models;

public class Album
{
    public required int Id { get; set; }

    public required string Title { get; set; }

    public required double Rating { get; set; }

    public required string CoverLink { get; set; }
}
