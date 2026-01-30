using DesafioAssino.Application.DTOs;
using DesafioAssino.Domain.Enums;

namespace DesafioAssino.Application.Interfaces;

public interface IListarTarefasService{
    Task<IReadOnlyList<TarefaResponse>> ExecutarAsync(EnumStatus? status, CancellationToken cancellationToken);
}