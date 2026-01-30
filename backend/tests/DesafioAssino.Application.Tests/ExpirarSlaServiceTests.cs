using DesafioAssino.Application.Interfaces;
using DesafioAssino.Application.Services;
using DesafioAssino.Domain.Entities;
using DesafioAssino.Domain.Enums;
using FluentAssertions;
using Moq;
using Xunit;

namespace DesafioAssino.Application.Tests;

public class ExpirarSlaServiceTests{
    private readonly Mock<ITarefaRepository> _repositoryMock = new();
    private readonly Mock<INotificacaoService> _notificacaoMock = new();

    [Fact]
    public async Task Deve_expirar_tarefas_vencidas_e_notificar(){

        var tarefaVencida = new TarefaItem("Vencida", 1, "file.pdf");
        var tarefasVencidas = new List<TarefaItem> { tarefaVencida };

        _repositoryMock
            .Setup(r => r.ObterTarefasComSlaVencidoAsync(It.IsAny<DateTime>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(tarefasVencidas);

        var service = new ExpirarSlaService(_repositoryMock.Object, _notificacaoMock.Object);

        await service.ExpirarAsync(CancellationToken.None);

        tarefaVencida.Status.Should().Be(EnumStatus.Expirada);
        
        _repositoryMock.Verify(
            r => r.SaveChangesAsync(It.IsAny<CancellationToken>()), 
            Times.Once);

        _notificacaoMock.Verify(
            n => n.NotificarExpiracaoAsync(tarefaVencida, It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task Nao_deve_fazer_nada_se_nao_houver_tarefas_vencidas(){
        _repositoryMock
            .Setup(r => r.ObterTarefasComSlaVencidoAsync(It.IsAny<DateTime>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<TarefaItem>());

        var service = new ExpirarSlaService(_repositoryMock.Object, _notificacaoMock.Object);

        await service.ExpirarAsync(CancellationToken.None);

        _repositoryMock.Verify(
            r => r.SaveChangesAsync(It.IsAny<CancellationToken>()), 
            Times.Never);

        _notificacaoMock.Verify(
            n => n.NotificarExpiracaoAsync(It.IsAny<TarefaItem>(), It.IsAny<CancellationToken>()),
            Times.Never);
    }
}
