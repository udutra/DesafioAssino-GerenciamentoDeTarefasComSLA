using DesafioAssino.Application.Interfaces;
using DesafioAssino.Domain.Entities;
using DesafioAssino.Domain.Exceptions;

namespace DesafioAssino.Application.Services;

public class ObterPendentesService(ITarefaRepository tarefaRepository) : IObterPendentesService{
    public async Task<IReadOnlyList<TarefaItem>> ExecutarAsync(CancellationToken cancellationToken){
        var tarefas = await tarefaRepository.ObterPendentesAsync(cancellationToken);

        return tarefas ?? throw new DomainException("Nenhuma tarefa pendente.");
    }
}