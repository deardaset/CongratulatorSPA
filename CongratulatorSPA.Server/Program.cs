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
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;
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

//FluentValidation
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<CreatePersonValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<UpdatePersonValidator>();

//DbContext
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

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
