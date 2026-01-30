using DesafioAssino.Application.Interfaces;
using DesafioAssino.Application.Services;
using DesafioAssino.Domain;
using DesafioAssino.Domain.Entities;
using DesafioAssino.Domain.Enums;
using DesafioAssino.Domain.Exceptions;
using FluentAssertions;
using Moq;
using Xunit;

namespace DesafioAssino.Application.Tests;

public class ConcluirTarefaServiceTests{

    private readonly Mock<ITarefaRepository> _repositoryMock = new();

    [Fact]
    public async Task Deve_concluir_tarefa_com_sucesso(){
        var tarefa = new TarefaItem("Teste", 2, "a.pdf");

        _repositoryMock
            .Setup(r => r.ObterPorIdAsync(tarefa.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(tarefa);

        var service = new ConcluirTarefaService(_repositoryMock.Object);

        await service.ExecutarAsync(tarefa.Id, CancellationToken.None);

        tarefa.Status.Should().Be(EnumStatus.Concluida);
        tarefa.DataConclusao.Should().NotBeNull();

        _repositoryMock.Verify(
            r => r.SaveChangesAsync(It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task Deve_lancar_excecao_quando_tarefa_nao_existir(){
        _repositoryMock
            .Setup(r => r.ObterPorIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((TarefaItem?)null);

        var service = new ConcluirTarefaService(_repositoryMock.Object);

        var act = async () => await service.ExecutarAsync(Guid.NewGuid(), CancellationToken.None);

        await act.Should().ThrowAsync<DomainException>()
            .WithMessage("Tarefa não encontrada.");
    }

    [Fact]
    public async Task Deve_lancar_excecao_ao_tentar_concluir_tarefa_expirada()
    {
        var tarefa = new TarefaItem("Teste", 2, "a.pdf");
        tarefa.MarcarComoExpirada();

        _repositoryMock
            .Setup(r => r.ObterPorIdAsync(tarefa.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(tarefa);

        var service = new ConcluirTarefaService(_repositoryMock.Object);

        var act = async () => await service.ExecutarAsync(tarefa.Id, CancellationToken.None);

        await act.Should().ThrowAsync<DomainException>()
            .WithMessage("Não é possível concluir uma tarefa expirada.");
        
        _repositoryMock.Verify(
            r => r.SaveChangesAsync(It.IsAny<CancellationToken>()), 
            Times.Never);
    }
}