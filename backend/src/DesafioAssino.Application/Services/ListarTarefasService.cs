using DesafioAssino.Application.DTOs;
using DesafioAssino.Application.Interfaces;
using DesafioAssino.Domain;
using DesafioAssino.Domain.Enums;

namespace DesafioAssino.Application.Services;

public sealed class ListarTarefasService(ITarefaRepository tarefaRepository) : IListarTarefasService{
    public async Task<IReadOnlyList<TarefaResponse>> ExecutarAsync(EnumStatus? status, CancellationToken cancellationToken){
        var tarefas = await tarefaRepository.ListarAsync(status, cancellationToken);

        return tarefas.Select(t => new TarefaResponse{
            NumTarefa = t.NumTarefa,
            Titulo = t.Titulo,
            Status = t.Status,
            DataCriacao = t.DataCriacao,
            DataConclusao = t.DataConclusao,
            DataExpiracao = t.DataExpiracao
        }).ToList();
    }
}