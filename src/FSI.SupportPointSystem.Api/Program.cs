using FSI.SupportPointSystem.Application;
using FSI.SupportPointSystem.Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using OpenApiModels = global::Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// --- NOVO: Configuração do Kestrel para HTTPS ---
// --- CONFIGURAÇÃO CORRETA PARA DOCKER NO AZURE ---
builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(8080); 

    options.ListenAnyIP(443, listenOptions =>
    {
        var certPath = "/app/certs/fsi-checkvisit-api.westus2.cloudapp.azure.com.pfx";
        var certPassword = "CheckVisit2026!";
        listenOptions.UseHttps(certPath, certPassword);
    });
});

// 1. Configuração de CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("WebAppPolicy", policy =>
    {
        policy.WithOrigins(
            "https://proud-island-0aa82ae0f.6.azurestaticapps.net", // Azure Front
            "http://44.195.62.176:3000",                            // AWS EC2
            "http://localhost:3000"                                 // Local Dev
        )
        .AllowAnyMethod()
        .AllowAnyHeader();
    });
});

// 2. Injeção de Dependência
builder.Services.AddApplication();
builder.Services.AddInfrastructure();

// 3. Configuração JWT
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
    // Agora que temos SSL, você pode mudar para 'true' em produção se desejar
    x.RequireHttpsMetadata = false; 
    x.SaveToken = true;
    x.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = !string.IsNullOrEmpty(issuer),
        ValidIssuer = issuer,
        ValidateAudience = !string.IsNullOrEmpty(audience),
        ValidAudience = audience,
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero
    };
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// 4. Configuração do Swagger
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiModels.OpenApiInfo
    {
        Title = "FSI Support Point System API",
        Version = "v1",
        Description = "API do projeto CheckVisit integrada ao Azure SSL"
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

// --- Middleware Pipeline ---

app.UseCors("WebAppPolicy");

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "FSI SupportPoint V1");
    c.RoutePrefix = string.Empty;
});

// Reativar agora que o certificado está configurado
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
