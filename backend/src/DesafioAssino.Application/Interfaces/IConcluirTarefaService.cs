namespace DesafioAssino.Application.Interfaces;

public interface IConcluirTarefaService{
    Task ExecutarAsync(Guid tarefaId, CancellationToken cancellationToken);
}