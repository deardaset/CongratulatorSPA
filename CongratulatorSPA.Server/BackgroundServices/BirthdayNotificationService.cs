using CongratulatorSPA.Server.Interfaces.Repositories;
using CongratulatorSPA.Server.Interfaces.Services;

namespace CongratulatorSPA.Server.BackgroundServices
{
    public class BirthdayNotificationService(IServiceScopeFactory scopeFactory, ILogger<BirthdayNotificationService> logger) : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                await WaitUntilMorningAsync(cancellationToken);
                await SendNotificationsAsync(cancellationToken);
            }
        }
        private async Task SendNotificationsAsync(CancellationToken cancellationToken)
        {
            using var scope = scopeFactory.CreateScope();
            var repository = scope.ServiceProvider.GetRequiredService<IPersonRepository>();
            var emailService = scope.ServiceProvider.GetRequiredService<IEmailService>();

            var people = await repository.GetTodaysBirthdaysAsync(cancellationToken);

            foreach (var person in people)
            {
                try
                {
                    await emailService.SendBirthdayAsync(person.Email!, person.Name, cancellationToken);
                    logger.LogInformation($"Письмо отправлено {person.Name}");
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, $"Ошибка отправки письма {person.Name}");
                }
            }
        }
        private async Task WaitUntilMorningAsync(CancellationToken cancellationToken)
        {
            var now = DateTime.UtcNow;
            var nextRun = DateTime.Today.AddHours(9);
            if (now > nextRun)
                nextRun = nextRun.AddDays(1);

            await Task.Delay(/*extRun - now*/TimeSpan.FromSeconds(10), cancellationToken);
        }
    }
}
