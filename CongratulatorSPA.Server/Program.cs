using CongratulatorSPA.Server.AutoMapperProfiles;
using CongratulatorSPA.Server.BackgroundServices;
using CongratulatorSPA.Server.Data;
using CongratulatorSPA.Server.Exceptions;
using CongratulatorSPA.Server.Extensions;
using CongratulatorSPA.Server.Interfaces.Repositories;
using CongratulatorSPA.Server.Interfaces.Services;
using CongratulatorSPA.Server.Models.Responses;
using CongratulatorSPA.Server.Repositories;
using CongratulatorSPA.Server.Services;
using CongratulatorSPA.Server.Validators;
using EFCoreSecondLevelCacheInterceptor;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Text.Json.Serialization;

CultureInfo.DefaultThreadCurrentCulture = CultureInfo.InvariantCulture;
CultureInfo.DefaultThreadCurrentUICulture = CultureInfo.InvariantCulture;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(
                    new JsonStringEnumConverter());
    });

//FluentValidation
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<CreatePersonValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<UpdatePersonValidator>();

//Caching
builder.Services.AddEFSecondLevelCache(options =>
    options.UseMemoryCacheProvider()
        .CacheAllQueries(CacheExpirationMode.Absolute, TimeSpan.FromMinutes(5))
        .ConfigureLogging(true)
);

//DbContext
builder.Services.AddDbContext<AppDbContext>((provider, options) =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
        .AddInterceptors(provider.GetRequiredService<SecondLevelCacheInterceptor>())
);

//Services
builder.Services.AddApplicationServices();

//AutoMapper
builder.Services.AddAutoMapper(cfg => cfg.AddProfile<PersonProfile>());

//ExceptionHandler
builder.Services.AddExceptionHandler<CongratulatorExceptionHandler>();
builder.Services.AddProblemDetails();

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

app.UseExceptionHandler();

app.MapControllers();

app.MapFallbackToFile("/index.html");

app.Run();
