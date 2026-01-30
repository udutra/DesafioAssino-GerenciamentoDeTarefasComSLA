using DesafioAssino.Application.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace DesafioAssino.Infrastructure.BackgroundServices;

public class SlaExpirationBackgroundService(IServiceScopeFactory scopeFactory) : BackgroundService{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using var scope = scopeFactory.CreateScope();
            var service = scope.ServiceProvider
                .GetRequiredService<IExpirarSlaService>();

            await service.ExpirarAsync(stoppingToken);

            await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
        }
    }
}
