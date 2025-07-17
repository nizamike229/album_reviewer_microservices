using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using ReviewService.Domain.Models;
using ReviewService.Domain.Repositories;
using ReviewService.Infrastructure.Persistence;

namespace ReviewService.Infrastructure.Repositories;

public class ReviewRepository : IReviewRepository
{
    private readonly ReviewDbContext _context;

    public ReviewRepository(ReviewDbContext context)
    {
        _context = context;
    }

    public async Task<string> Post(Review review)
    {
        await _context.Reviews.AddAsync(review);
        await _context.SaveChangesAsync();
        return "Review was published successfully!";
    }

    public async Task<List<Review>> GetAll(string mbId)
    {
        return await _context.Reviews.Where(r => r.MbId == mbId).ToListAsync();
    }

    public async Task<AverageReviewModel> GetAverage(string mbId)
    {
        var reviews = await _context.Reviews
            .Where(r => r.MbId == mbId)
            .ToListAsync();

        var averageRating = reviews.Average(r => r.Rating);

        var trackRatings = new Dictionary<int, List<double>>();

        foreach (var review in reviews)
        {
            if (string.IsNullOrWhiteSpace(review.TrackRatings))
                continue;

            var deserialized = JsonSerializer.Deserialize<Dictionary<int, double>>(review.TrackRatings);
            if (deserialized == null)
                continue;

            foreach (var (trackNumber, rating) in deserialized)
            {
                if (!trackRatings.ContainsKey(trackNumber))
                    trackRatings[trackNumber] = [];

                trackRatings[trackNumber].Add(rating);
            }
        }

        var averageTrackListReview = trackRatings.ToDictionary(
            kvp => kvp.Key,
            kvp => kvp.Value.Average()
        );

        return new AverageReviewModel
        {
            Average = (double)averageRating!,
            TrackRatings = averageTrackListReview
        };
    }

    public async Task<List<Review>> GetPersonal(string userId)
    {
        return await _context.Reviews.Where(r => r.UserId == userId).ToListAsync();
    }
}