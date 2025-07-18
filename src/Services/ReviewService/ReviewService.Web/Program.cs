using ReviewService.Extensions;
using ReviewService.Infrastructure.Persistence;
using ReviewService.Middleware;

var builder = WebApplication.CreateBuilder(args);

if (builder.Environment.IsDevelopment())
    builder.Configuration.AddUserSecrets<Program>();

builder.Services.AddControllers();
builder.Services.AddSwaggerGen();
builder.Services.AddGrpcDependencies();
builder.Services.AddDbContext<ReviewDbContext>();
builder.AddJwtAuthentication();
builder.Services.AddServices();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ExceptionHandlerMiddleware>();
app.ConfigureCors();
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();