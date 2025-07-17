using ReviewService.Domain.Models;

namespace ReviewService.Domain.Repositories;

public interface IReviewRepository
{
    Task<string> Post(Review review);
    Task<List<Review>> GetAll(string mbId);
    Task<AverageReviewModel> GetAverage(string mbId);
    Task<List<Review>> GetPersonal();
}