using CongratulatorSPA.Server.BackgroundServices;
using CongratulatorSPA.Server.Interfaces.Repositories;
using CongratulatorSPA.Server.Interfaces.Services;
using CongratulatorSPA.Server.Models.Responses;
using CongratulatorSPA.Server.Repositories;
using CongratulatorSPA.Server.Services;
using Quartz;

namespace CongratulatorSPA.Server.Extensions
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            //Services
            services.Scan(scan => scan
                .FromAssemblyOf<PersonRepository>()
                .AddClasses(classes => classes.InNamespaces(
                    "CongratulatorSPA.Server.Services",
                    "CongratulatorSPA.Server.Repositories"))
                .AsMatchingInterface()
            .WithScopedLifetime());

            services.AddScoped<IGetPeopleService<PersonResponse>, GetPeopleService>();
            services.AddSingleton<IStorageService, StorageService>();
            services.AddSingleton<IEmailService, EmailService>();

            //BackgroundService
            services.AddQuartz(q =>
            {
                var jobKey = new JobKey("BirthdayNotification");
                q.AddJob<BirthdayNotificationService>(options => options.WithIdentity(jobKey));

                q.AddTrigger(options => options
                    .ForJob(jobKey)
                    .WithIdentity("BirthdayNotification-trigger")
                    .WithCronSchedule("0 0 9 * * ?")
                );
            });

            services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);

            return services;
        }
    }
}
