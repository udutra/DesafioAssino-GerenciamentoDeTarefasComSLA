
using DesafioAssino.Application.Interfaces;
using DesafioAssino.Domain.Entities;
using DesafioAssino.Domain.Exceptions;

namespace DesafioAssino.Application.Services;

public class ObterPorNumTarefaService(ITarefaRepository tarefaRepository) : IObterPorNumTarefaService{
    public async Task<TarefaItem> ExecutarAsync(int numTarefa, CancellationToken cancellationToken){
        var tarefa = await tarefaRepository.ObterPorNumTarefaAsync(numTarefa, cancellationToken);

        return tarefa ?? throw new DomainException("Nenhuma tarefa pendente.");
    }
}