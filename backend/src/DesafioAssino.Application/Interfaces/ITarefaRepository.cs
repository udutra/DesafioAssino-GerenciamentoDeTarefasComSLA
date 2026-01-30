using DesafioAssino.Domain.Entities;
using DesafioAssino.Domain.Enums;

namespace DesafioAssino.Application.Interfaces;

public interface ITarefaRepository{
    Task AddAsync(TarefaItem tarefa, CancellationToken cancellationToken);
    Task<IReadOnlyList<TarefaItem>> ListarAsync(EnumStatus? status, CancellationToken cancellationToken);
    Task<IReadOnlyList<TarefaItem>> ObterPendentesAsync(CancellationToken cancellationToken);
    Task<List<TarefaItem>> ObterTarefasComSlaVencidoAsync(DateTime agora, CancellationToken cancellationToken);
    Task SaveChangesAsync(CancellationToken cancellationToken);
    Task<TarefaItem?> ObterPorIdAsync(Guid id, CancellationToken cancellationToken);
    Task<TarefaItem?> ObterPorNumTarefaAsync(int numTarefa, CancellationToken cancellationToken);
}