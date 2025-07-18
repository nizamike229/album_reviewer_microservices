using ReviewService.Application.Services;
using ReviewService.Domain.Models;
using ReviewService.Domain.Models.Response;
using ReviewService.Domain.Repositories;

namespace ReviewService.Infrastructure.Services;

public class ReviewService : IReviewService
{
    private readonly IReviewRepository _reviewRepository;

    public ReviewService(IReviewRepository reviewRepository)
    {
        _reviewRepository = reviewRepository;
    }

    public Task<string> Post(Review review)
    {
        return _reviewRepository.Post(review);
    }

    public Task<List<ReviewResponse>> GetAll(string mbId)
    {
        return _reviewRepository.GetAll(mbId);
    }

    public Task<AverageReviewModel> GetAverage(string mbId)
    {
        return _reviewRepository.GetAverage(mbId);
    }

    public Task<List<Review>> GetPersonal(string userId)
    {
        return _reviewRepository.GetPersonal(userId);
    }
}