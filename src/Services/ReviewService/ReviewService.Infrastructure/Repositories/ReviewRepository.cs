using ReviewService.Domain.Models;
using ReviewService.Domain.Repositories;
using ReviewService.Infrastructure.Persistence;

namespace ReviewService.Infrastructure.Repositories;

public class ReviewRepository : IReviewRepository
{
    private readonly ReviewDbContext  _context;

    public ReviewRepository(ReviewDbContext context)
    {
        _context = context;
    }

    public Task<string> Post(Review review)
    {
        throw new NotImplementedException();
    }

    public Task<List<Review>> GetAll(string mbId)
    {
        throw new NotImplementedException();
    }

    public Task<AverageReviewModel> GetAverage(string mbId)
    {
        throw new NotImplementedException();
    }

    public Task<List<Review>> GetPersonal()
    {
        throw new NotImplementedException();
    }
}