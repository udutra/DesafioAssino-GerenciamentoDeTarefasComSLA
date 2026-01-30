using DesafioAssino.Application.Interfaces;
using DesafioAssino.Domain.Exceptions;

namespace DesafioAssino.Application.Services;

public sealed class ConcluirTarefaService(ITarefaRepository tarefaRepository) : IConcluirTarefaService{

    public async Task ExecutarAsync(Guid tarefaId, CancellationToken cancellationToken){
        var tarefa = await tarefaRepository.ObterPorIdAsync(tarefaId, cancellationToken);

        if (tarefa is null)
            throw new DomainException("Tarefa não encontrada.");

        tarefa.Concluir();

        await tarefaRepository.SaveChangesAsync(cancellationToken);
    }
}