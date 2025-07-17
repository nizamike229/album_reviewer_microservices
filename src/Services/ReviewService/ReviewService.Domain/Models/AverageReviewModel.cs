namespace ReviewService.Domain.Models;

public class AverageReviewModel
{
    public required double Average { get; set; }
    public required Dictionary<int,double> TrackRatings { get; set; }
}