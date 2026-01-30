using DesafioAssino.Application.DTOs;
using DesafioAssino.Application.Interfaces;
using DesafioAssino.Domain.Entities;
using FluentValidation;

namespace DesafioAssino.Application.Services;

public sealed class CriarTarefaService(ITarefaRepository tarefaRepository, IFileStorageService fileStorageService, IValidator<CriarTarefaRequest> validator) : ICriarTarefaService{
    public async Task<TarefaResponse> ExecutarAsync(CriarTarefaRequest request, CancellationToken cancellationToken){
        await validator.ValidateAndThrowAsync(request, cancellationToken);
        var arquivoPath = await fileStorageService.SalvarAsync(request.ArquivoNome, request.ArquivoConteudo, cancellationToken);

        var tarefa = new TarefaItem(request.Titulo, request.SlaHoras, arquivoPath);

        await tarefaRepository.AddAsync(tarefa, cancellationToken);

        return new TarefaResponse{
            NumTarefa = tarefa.NumTarefa,
            Titulo = tarefa.Titulo,
            Status = tarefa.Status,
            DataCriacao = tarefa.DataCriacao,
            DataExpiracao = tarefa.DataExpiracao
        };
    }
}