using DesafioAssino.Application.Interfaces;

namespace DesafioAssino.Infrastructure.Storage;

public sealed class LocalFileStorageService : IFileStorageService{
    private readonly string _basePath;

    public LocalFileStorageService(){
        _basePath = Path.Combine(Directory.GetCurrentDirectory(), "uploads");

        if (!Directory.Exists(_basePath))
            Directory.CreateDirectory(_basePath);
    }

    public async Task<string> SalvarAsync(string nomeArquivo, byte[] conteudo, CancellationToken cancellationToken){
        var nomeSeguro = $"{Guid.NewGuid()}_{nomeArquivo}";
        var caminhoCompleto = Path.Combine(_basePath, nomeSeguro);

        await File.WriteAllBytesAsync(caminhoCompleto, conteudo, cancellationToken);

        return nomeSeguro;
    }
}