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

//builder.Services.AddCors(options =>
//{
//    options.AddPolicy("PermitirAngular", policy =>
//    {
//        // El puerto donde corre ng serve
//        policy.WithOrigins("http://sadt-app.s3-website.us-east-2.amazonaws.com")
//              .AllowAnyHeader()
//              .AllowAnyMethod();
//    });
//});

// 4. Configuración de Seguridad: Autenticación con JWT (CONFIGURA EL ESQUEMA Y LA VALIDACIÓN)
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    //configura los paramtros de validación del token JWT
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            /*El Issuer (Emisor) es la entidad que generó el token. Al activarlo,
            obligas a la API a verificar el origen. Con ValidIssuer le dices exactamente
            qué nombre esperas (leyéndolo de la configuración). Si alguien intenta usar un
            token generado por un servidor distinto, la API lo rechaza.*/
            ValidateIssuer = true,
            /*l Audience (Audiencia) indica para qué cliente está destinado el token.
            Esto asegura que el token fue emitido específicamente para tu aplicación frontend.*/
            ValidateAudience = true,
            /*Activa el control del tiempo. La API leerá la fecha de expiración interna del token 
             y bloqueará el acceso automáticamente si el tiempo ya transcurrió.*/
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            /*Le exige a la API que valide matemáticamente la firma del token utilizando la clave secreta simétrica (Jwt:Key).
            Si un atacante intercepta un token válido e intenta alterar su contenido (ejem, cambiarse el rol de usuario a administrador),
            la firma original se corrompe. Al recibirlo, la API usa esta clave para darse cuenta de la manipulación y rechaza la petición
            al instante.*/
            ValidAudience = builder.Configuration["Jwt:Audience"],
            // La clave secreta debe tener al menos 16 caracteres
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"] ?? string.Empty))
        };
    });

//DEFINE POLITICAS Y REQUISITOS
builder.Services.AddAuthorization();

var app = builder.Build();

// 5. Configurar el pipeline HTTP
//app.UseCors("PermitirAngular");

//ESTABLECE LA IDENTIDAD
app.UseAuthentication();
//EVALUA EL ACCESO
app.UseAuthorization();

app.MapControllers();

app.Run();
