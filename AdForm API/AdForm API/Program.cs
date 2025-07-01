using AdForm_API.AdFormDB;
using AdForm_API.Data;
using AdForm_API.Services;
using Microsoft.OpenApi.Models;
using System.Reflection;
using Path = System.IO.Path;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "AdForm SQL API",
        Description = "An ASP.NET Core Web API for AdForm SQL exercise"
    });
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    options.IncludeXmlComments(xmlPath);
});

builder.Services.AddDbContext<AdFormContext>();
builder.Services.AddScoped<AdFormService, AdFormService>();

builder.Services.AddGraphQLServer()
    .AddQueryType<Query>()
    .AddMutationType<Query>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapGraphQL("/graphql");

app.Run();
