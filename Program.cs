using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using TagAPI.Repositories.Impl;
using TagAPI.Repositories.Interfaces;
using TagAPI.Services.Impl;
using TagAPI.Services.Interfaces;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//repos
builder.Services.AddScoped<ITagRepo, TagRepo>();
builder.Services.AddScoped<ITagService, TagService>();

//AWS
var awsOptions = builder.Configuration.GetAWSOptions();
builder.Services.AddDefaultAWSOptions(awsOptions);
builder.Services.AddScoped<IDynamoDBContext, DynamoDBContext>();
builder.Services.AddAWSService<IAmazonDynamoDB>();

var app = builder.Build();



// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
