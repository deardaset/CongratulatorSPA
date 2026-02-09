using CongratulatorSPA.Server.Data;
using CongratulatorSPA.Server.Interfaces.Repositories;
using CongratulatorSPA.Server.Interfaces.Services;
using CongratulatorSPA.Server.Middleware;
using CongratulatorSPA.Server.Models.Responses;
using CongratulatorSPA.Server.Repositories;
using CongratulatorSPA.Server.Services;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(
                    new JsonStringEnumConverter());
    });

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IPersonRepository, PersonRepository>();
builder.Services.AddScoped<ICreatePersonService, CreatePersonService>();
builder.Services.AddScoped<IUpdatePersonService, UpdatePersonService>();
builder.Services.AddScoped<IDeletePersonService, DeletePersonService>();
builder.Services.AddScoped<IGetPersonService, GetPersonService>();
builder.Services.AddScoped<IGetPeopleService<PersonResponse>, GetPeopleService>();
builder.Services.AddScoped<IGetUpcomingService<PersonResponse>, GetUpcomingService>();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

app.UseDefaultFiles();
app.MapStaticAssets();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseMiddleware<ExceptionHandlerMiddleware>();

app.MapControllers();

app.MapFallbackToFile("/index.html");

app.Run();
