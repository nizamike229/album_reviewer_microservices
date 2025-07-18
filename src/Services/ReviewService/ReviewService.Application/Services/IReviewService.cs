using ReviewService.Domain.Models;
using ReviewService.Domain.Models.Response;

namespace ReviewService.Application.Services;

public interface IReviewService
{
    Task<string> Post(Review review);
    Task<List<ReviewResponse>> GetAll(string mbId);
    Task<AverageReviewModel> GetAverage(string mbId);
    Task<List<Review>> GetPersonal(string userId);
}