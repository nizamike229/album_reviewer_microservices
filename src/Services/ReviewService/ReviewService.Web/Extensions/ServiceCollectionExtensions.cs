using ReviewService.Domain.Repositories;
using ReviewService.Infrastructure.Repositories;

namespace ReviewService.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddServices(this IServiceCollection services)
    {
        services.AddScoped<IReviewRepository, ReviewRepository>();
    }
}