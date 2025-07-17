using System.Text.Json;
using ReviewService.Domain.Models;
using ReviewService.Models;

namespace ReviewService.Mapping;

public static class ReviewMapper
{
    public static Review ToReview(this ReviewRequest request,string userId)
    {
        return new Review
        {
            Id = Guid.NewGuid().ToString(),
            Rating = request.Rating,
            TrackRatings = JsonSerializer.Serialize(request.TrackRatings),
            Description = request.Description,
            UserId = userId,
            MbId = request.MbId
        };
    }
}