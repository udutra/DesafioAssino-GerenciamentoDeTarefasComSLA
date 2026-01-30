using DesafioAssino.Domain.Entities;

namespace DesafioAssino.Application.Interfaces;

public interface IObterPendentesService{
    Task<IReadOnlyList<TarefaItem>> ExecutarAsync(CancellationToken cancellationToken);
}