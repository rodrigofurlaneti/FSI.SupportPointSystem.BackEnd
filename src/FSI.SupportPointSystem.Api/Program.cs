using FSI.SupportPointSystem.Application;
using FSI.SupportPointSystem.Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using OpenApiModels = global::Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(8080); 
    options.ListenAnyIP(443, listenOptions =>
    {
        var certPath = builder.Configuration["Kestrel:Certificates:Default:Path"];
        var certPassword = builder.Configuration["Kestrel:Certificates:Default:Password"];
        if (!string.IsNullOrEmpty(certPath) && System.IO.File.Exists(certPath))
        {
            listenOptions.UseHttps(certPath, certPassword);
        }
    });
});

// 1. Configuração de CORS lendo do appsettings.json
var allowedOrigins = builder.Configuration.GetSection("AllowedOrigins").Get<string[]>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("WebAppPolicy", policy =>
    {
        policy.WithOrigins(allowedOrigins) 
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// 2. Injeção de Dependência
builder.Services.AddApplication();
builder.Services.AddInfrastructure();

// 3. Configuração JWT (Lendo do JSON)
var jwtSettings = builder.Configuration.GetSection("Jwt");
var secretKey = jwtSettings["Secret"] ?? throw new InvalidOperationException("JWT Secret is missing!");
var key = Encoding.ASCII.GetBytes(secretKey);

builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(x =>
{
    x.RequireHttpsMetadata = true; // Agora temos SSL ativo
    x.SaveToken = true;
    x.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = true,
        ValidIssuer = jwtSettings["Issuer"],
        ValidateAudience = true,
        ValidAudience = jwtSettings["Audience"],
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero
    };
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// --- Middleware Pipeline ---
app.UseCors("WebAppPolicy");
app.UseSwagger();
app.UseSwaggerUI(c => {
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "FSI SupportPoint V1");
    c.RoutePrefix = string.Empty;
});

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
