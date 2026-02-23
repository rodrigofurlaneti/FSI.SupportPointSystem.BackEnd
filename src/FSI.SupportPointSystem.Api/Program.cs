using FSI.SupportPointSystem.Application;
using FSI.SupportPointSystem.Infrastructure;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddApplication();
builder.Services.AddInfrastructure();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "FSI Support Point System API",
        Version = "v1",
        Description = "API de monitoramento de visitas geolocalizadas."
    });
});
var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "FSI SupportPoint V1");
    c.RoutePrefix = string.Empty; 
});
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();