using DesafioAssino.Application.DTOs;

namespace DesafioAssino.Application.Interfaces;

public interface ICriarTarefaService
{
    Task<TarefaResponse> ExecutarAsync(CriarTarefaRequest request, CancellationToken cancellationToken);
}
