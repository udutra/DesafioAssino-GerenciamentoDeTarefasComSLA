using DesafioAssino.Domain.Entities;

namespace DesafioAssino.Application.Interfaces;

public interface IObterPorNumTarefaService{
    Task<TarefaItem> ExecutarAsync(int numTarefa, CancellationToken cancellationToken);
}