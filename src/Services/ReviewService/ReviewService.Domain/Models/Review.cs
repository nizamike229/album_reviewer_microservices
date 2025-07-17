namespace ReviewService.Domain.Models;

public class Review
{
    public required string Id { get; set; }

    public required double? Rating { get; set; }

    public required string TrackRatings { get; set; }

    public required string Description { get; set; }

    public required string UserId { get; set; }

    public required string MbId { get; set; }
}
