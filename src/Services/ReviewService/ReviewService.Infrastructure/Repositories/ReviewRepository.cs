using System.Globalization;
using System.Text.Json;
using GrpcAuthService;
using Microsoft.EntityFrameworkCore;
using ReviewService.Domain.Models;
using ReviewService.Domain.Models.Response;
using ReviewService.Domain.Repositories;
using ReviewService.Infrastructure.Persistence;

namespace ReviewService.Infrastructure.Repositories;

public class ReviewRepository : IReviewRepository
{
    private readonly ReviewDbContext _context;
    private readonly GetUsernameById.GetUsernameByIdClient _getUsernameByIdClient;

    public ReviewRepository(ReviewDbContext context, GetUsernameById.GetUsernameByIdClient getUsernameByIdClient)
    {
        _context = context;
        _getUsernameByIdClient = getUsernameByIdClient;
    }

    public async Task<string> Post(Review review)
    {
        var unescaped = JsonSerializer.Deserialize<string>(review.TrackRatings);
        var raw = JsonSerializer.Deserialize<Dictionary<string, string>>(unescaped!);
        var cleaned = raw!.ToDictionary(
            kvp => kvp.Key,
            kvp => double.TryParse(kvp.Value, NumberStyles.Any, CultureInfo.InvariantCulture, out var d)
                ? d
                : throw new Exception($"Invalid number: {kvp.Value}")
        );
        review.TrackRatings = JsonSerializer.Serialize(cleaned);

        await _context.Reviews.AddAsync(review);

        await _context.SaveChangesAsync();
        return "Review was published successfully!";
    }

    public async Task<List<ReviewResponse>> GetAll(string mbId)
    {
        var reviews = await _context.Reviews.Where(r => r.MbId == mbId).ToListAsync();
        var reviewResponse = new List<ReviewResponse>();
        foreach (var review in reviews)
        {
            var username = (await _getUsernameByIdClient.GetUsernameAsync(new DataRequest { UserId = review.UserId }))
                .Username;
            reviewResponse.Add(new ReviewResponse
            {
                Id = review.Id,
                Rating = review.Rating,
                TrackRatings = review.TrackRatings,
                Description = review.Description,
                UserId = review.UserId,
                Username = username,
                MbId = review.MbId
            });
        }

        return reviewResponse;
    }

    public async Task<AverageReviewModel> GetAverage(string mbId)
    {
        var reviews = await _context.Reviews
            .Where(r => r.MbId == mbId)
            .ToListAsync();
        if (reviews.Count == 0)
            throw new Exception("No reviews found");

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