using ReviewService.Domain.Models;
using ReviewService.Domain.Models.Response;

namespace ReviewService.Domain.Repositories;

public interface IReviewRepository
{
    Task<string> Post(Review review);
    Task<List<ReviewResponse>> GetAll(string mbId);
    Task<AverageReviewModel> GetAverage(string mbId);
    Task<List<Review>> GetPersonal(string userId);
}