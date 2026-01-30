using DesafioAssino.Domain.Entities;

namespace DesafioAssino.Application.Interfaces;

public interface INotificacaoService
{
    Task NotificarExpiracaoAsync(TarefaItem tarefa, CancellationToken cancellationToken);
}
