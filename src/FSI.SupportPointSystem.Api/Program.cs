using FSI.SupportPointSystem.Application;
using FSI.SupportPointSystem.Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using OpenApiModels = global::Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddCors(options =>
{
    options.AddPolicy("WebAppPolicy", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});
builder.Services.AddApplication();
builder.Services.AddInfrastructure();
var jwtSettings = builder.Configuration.GetSection("Jwt");
var secretKey = jwtSettings.GetValue<string>("Secret")
    ?? throw new InvalidOperationException("JWT Secret is missing!");
var issuer = jwtSettings.GetValue<string>("Issuer");
var audience = jwtSettings.GetValue<string>("Audience");
var key = Encoding.ASCII.GetBytes(secretKey);
builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(x =>
{
    x.RequireHttpsMetadata = false;
    x.SaveToken = true;
    x.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = false,
        ValidateAudience = false,
        ClockSkew = TimeSpan.Zero
    };
});
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiModels.OpenApiInfo
    {
        Title = "FSI Support Point System API",
        Version = "v1"
    });
    c.AddSecurityDefinition("Bearer", new OpenApiModels.OpenApiSecurityScheme
    {
        Description = "JWT Authorization header. Digite: 'Bearer {seu_token}'",
        Name = "Authorization",
        In = OpenApiModels.ParameterLocation.Header,
        Type = OpenApiModels.SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });
    c.AddSecurityRequirement(new OpenApiModels.OpenApiSecurityRequirement
    {
        {
            new OpenApiModels.OpenApiSecurityScheme
            {
                Reference = new OpenApiModels.OpenApiReference
                {
                    Type = OpenApiModels.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

var app = builder.Build();
app.UseCors("WebAppPolicy");
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "FSI SupportPoint V1");
    c.RoutePrefix = string.Empty;
});
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();