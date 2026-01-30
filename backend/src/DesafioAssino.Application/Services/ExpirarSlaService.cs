using DesafioAssino.Application.Interfaces;
namespace DesafioAssino.Application.Services;

public class ExpirarSlaService(ITarefaRepository tarefaRepository, INotificacaoService notificacaoService) : IExpirarSlaService{
    public async Task ExpirarAsync(CancellationToken cancellationToken)
    {
        var agora = DateTime.UtcNow;

        var tarefasVencidas =
            await tarefaRepository.ObterTarefasComSlaVencidoAsync(
                agora,
                cancellationToken);

        if (tarefasVencidas.Count == 0)
            return;

        foreach (var tarefa in tarefasVencidas)
        {
            tarefa.MarcarComoExpirada();
            await notificacaoService.NotificarExpiracaoAsync(tarefa, cancellationToken);
        }

        await tarefaRepository.SaveChangesAsync(cancellationToken);
    }
}