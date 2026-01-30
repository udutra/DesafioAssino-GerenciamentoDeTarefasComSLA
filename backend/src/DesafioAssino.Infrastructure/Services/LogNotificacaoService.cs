using DesafioAssino.Application.Interfaces;
using DesafioAssino.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace DesafioAssino.Infrastructure.Services;

public class LogNotificacaoService(ILogger<LogNotificacaoService> logger) : INotificacaoService
{
    public Task NotificarExpiracaoAsync(TarefaItem tarefa, CancellationToken cancellationToken)
    {
        logger.LogWarning("ALERTA: A tarefa '{Titulo}' (ID: {Id}) expirou em {DataExpiracao}!", 
            tarefa.Titulo, tarefa.Id, tarefa.DataExpiracao);
        
        return Task.CompletedTask;
    }
}
