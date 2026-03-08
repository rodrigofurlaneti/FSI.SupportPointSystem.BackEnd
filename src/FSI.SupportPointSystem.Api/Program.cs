using FSI.SupportPointSystem.Application;
using FSI.SupportPointSystem.Infrastructure;
using FSI.SupportPointSystem.Infrastructure.Configuration; 
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// --- 1. Configuração do Servidor (Kestrel) ---
builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(8080); // Mantém a porta 8080 para HTTP
    options.ListenAnyIP(443, listenOptions =>
    {
        var certPath = builder.Configuration["Kestrel:Certificates:Default:Path"];
        var certPassword = builder.Configuration["Kestrel:Certificates:Default:Password"];

        if (!string.IsNullOrEmpty(certPath) && System.IO.File.Exists(certPath))
        {
            listenOptions.UseHttps(certPath, certPassword); // Usa certificado configurado
        }
        else
        {
            listenOptions.UseHttps(); // Certificado padrão de dev
        }
    });
});

// --- 2. Injeção de Dependências (Serviços) ---

// Configuração de CORS com a sua política WebAppPolicy
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

// Camadas da Arquitetura
builder.Services.AddApplication();
builder.Services.AddInfrastructure();

// Extensões de Configuração (Swagger e Segurança JWT)
builder.Services.AddSwaggerConfig(); // Sua nova classe SwaggerConfiguration
builder.Services.AddSecurity(builder.Configuration); // Sua nova classe SecurityConfiguration

builder.Services.AddControllers();

var app = builder.Build();

// --- 3. Middleware Pipeline (Ordem de Execução) ---

// Ativa o CORS antes de qualquer outro middleware de rota
app.UseCors("WebAppPolicy");

// Configuração visual do Swagger
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "FSI SupportPoint V1");
    c.RoutePrefix = string.Empty; // Define o Swagger como página inicial
});

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();