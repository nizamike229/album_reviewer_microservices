namespace ApiGateway.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddJwtBearerConfiguration(
        this WebApplicationBuilder builder)
    {
        builder.Services.AddAuthentication("Bearer")
            .AddJwtBearer("Bearer", options =>
            {
                options.Authority = "http://localhost:5232/api/auth";
                options.TokenValidationParameters.ValidateAudience = false;
                options.RequireHttpsMetadata = false;
            });
        builder.Services.AddAuthorization();
    }

    public static void AddYarpConfiguration(
        this WebApplicationBuilder builder)
    {
        builder.Services.AddReverseProxy()
            .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));
    }
}