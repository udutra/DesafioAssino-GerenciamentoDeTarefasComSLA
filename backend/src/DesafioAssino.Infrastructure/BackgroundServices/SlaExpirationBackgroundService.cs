using DesafioAssino.Application.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace DesafioAssino.Infrastructure.BackgroundServices;

public class SlaExpirationBackgroundService(
    IServiceScopeFactory scopeFactory,
    ILogger<SlaExpirationBackgroundService> logger)
    : BackgroundService{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation("SlaExpirationBackgroundService iniciado.");

        while (!stoppingToken.IsCancellationRequested)
        {
            logger.LogInformation("SlaExpirationBackgroundService rodando em {Time}", DateTime.Now);

            using var scope = scopeFactory.CreateScope();
            var service = scope.ServiceProvider.GetRequiredService<IExpirarSlaService>();

            await service.ExpirarAsync(stoppingToken);

            await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
        }

        logger.LogInformation("SlaExpirationBackgroundService cancelado.");
    }
}
