using CongratulatorSPA.Server.BackgroundServices;
using CongratulatorSPA.Server.Interfaces.Repositories;
using CongratulatorSPA.Server.Interfaces.Services;
using CongratulatorSPA.Server.Models.Responses;
using CongratulatorSPA.Server.Repositories;
using CongratulatorSPA.Server.Services;

namespace CongratulatorSPA.Server.Extensions
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            //Services
            services.AddScoped<IPersonRepository, PersonRepository>();
            services.AddScoped<ICreatePersonService, CreatePersonService>();
            services.AddScoped<IUpdatePersonService, UpdatePersonService>();
            services.AddScoped<IDeletePersonService, DeletePersonService>();
            services.AddScoped<IGetPersonService, GetPersonService>();
            services.AddScoped<IGetPeopleService<PersonResponse>, GetPeopleService>();
            services.AddScoped<IStorageService, StorageService>();
            services.AddScoped<IEmailService, EmailService>();

            //BackgroundService
            services.AddHostedService<BirthdayNotificationService>();

            return services;
        }
    }
}
