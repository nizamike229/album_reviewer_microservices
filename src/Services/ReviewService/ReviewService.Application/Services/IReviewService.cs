using ReviewService.Domain.Models;

namespace ReviewService.Application.Services;

public interface IReviewService
{
    Task<string> Post(Review review);
    Task<List<Review>> GetAll(string mbId);
    Task<AverageReviewModel> GetAverage(string mbId);
    Task<List<Review>> GetPersonal(string userId);
}