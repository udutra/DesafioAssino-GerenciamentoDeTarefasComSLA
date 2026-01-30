namespace DesafioAssino.Application.Interfaces;

public interface IFileStorageService{
    Task<string> SalvarAsync(string nomeArquivo, byte[] conteudo, CancellationToken cancellationToken);
}