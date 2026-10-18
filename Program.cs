using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using TagAPI.Repositories.Impl;
using TagAPI.Repositories.Interfaces;
using TagAPI.Services.Impl;
using TagAPI.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// 1. Inyección de Dependencias: Repositorios y Servicios
builder.Services.AddScoped<ITagRepo, TagRepo>();
builder.Services.AddScoped<ITagService, TagService>();
// nuevo servicio de Auth
builder.Services.AddScoped<IAuthService, AuthService>();

// 2. Configuración de AWS y DynamoDB
var awsOptions = builder.Configuration.GetAWSOptions();
builder.Services.AddDefaultAWSOptions(awsOptions);
builder.Services.AddAWSService<IAmazonDynamoDB>();
builder.Services.AddScoped<IDynamoDBContext, DynamoDBContext>();
builder.Services.AddScoped<IUsuarioRepo, UsuarioRepo>();

// 3. Configuración para AWS Lambda (Function URLs usan HttpApi)
builder.Services.AddAWSLambdaHosting(LambdaEventSource.HttpApi);

// 4. Configuración de Seguridad: Autenticación con JWT
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            // La clave secreta debe tener al menos 16 caracteres
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"] ?? string.Empty))
        };
    });

builder.Services.AddCors(options =>
{
    options.AddPolicy("PermitirAngular", policy =>
    {
        // El puerto donde corre ng serve
        policy.WithOrigins("http://localhost:4200")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});
builder.Services.AddAuthorization();

var app = builder.Build();

// 5. Configurar el pipeline HTTP
// (Swagger eliminado intencionalmente. Las pruebas se harán vía TagAPI.http)

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();