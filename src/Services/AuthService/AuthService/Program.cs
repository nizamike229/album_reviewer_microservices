using AuthService.Context;
using AuthService.Extensions;
using AuthService.GrpcServices;
using AuthService.Middlewares;

var builder = WebApplication.CreateBuilder(args);

if (builder.Environment.IsDevelopment())
    builder.Configuration.AddUserSecrets<Program>();

builder.Services.AddControllers();
builder.Services.AddSwaggerGen();
builder.Services.AddGrpc();
builder.Services.AddDbContext<AuthDbContext>();
builder.AddJwtAuthentication();
builder.Services.AddServices();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGrpcService<GrpcService>();
app.UseMiddleware<ExceptionHandlerMiddleware>();
app.ConfigureCors();
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();