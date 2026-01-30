namespace DesafioAssino.Application.Interfaces;

public interface IConcluirTarefaService{
    Task ExecutarAsync(int numTarefa, CancellationToken cancellationToken);
}