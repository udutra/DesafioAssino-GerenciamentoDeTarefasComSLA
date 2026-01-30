namespace DesafioAssino.Application.Interfaces;

public interface IExpirarSlaService{
    Task ExpirarAsync(CancellationToken cancellationToken);
}