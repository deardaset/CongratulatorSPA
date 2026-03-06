using CongratulatorSPA.Server.Interfaces.Repositories;
using CongratulatorSPA.Server.Interfaces.Services;
using Quartz;

namespace CongratulatorSPA.Server.BackgroundServices
{
    [DisallowConcurrentExecution]
    public class BirthdayNotificationService(IPersonRepository repository, IEmailService emailService, ILogger<BirthdayNotificationService> logger) : IJob
    {
        public async Task Execute(IJobExecutionContext context)
        {
            while (!context.CancellationToken.IsCancellationRequested)
            {
                var people = await repository.GetTodaysBirthdaysAsync(context.CancellationToken);

                foreach (var person in people)
                {
                    try
                    {
                        await emailService.SendBirthdayAsync(person.Email!, person.Name, context.CancellationToken);
                        logger.LogInformation("Письмо отправлено {Name}", person.Name);
                    }
                    catch (Exception ex)
                    {
                        logger.LogError(ex, "Ошибка отправки письма {Name}", person.Name);
                    }
                }
            }
        }
    }
}
