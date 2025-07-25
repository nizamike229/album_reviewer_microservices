using ApiGateway.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddSwaggerGen();
builder.AddJwtBearerConfiguration();
builder.AddYarpConfiguration();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.ConfigureCors();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapReverseProxy(proxyPipeline =>
{
    proxyPipeline.UseAuthentication();
    proxyPipeline.UseAuthorization();
    proxyPipeline.ConfigureCorsInPipeline();
});

app.MapControllers();

app.Run();