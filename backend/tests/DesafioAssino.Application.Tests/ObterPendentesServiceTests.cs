using DesafioAssino.Application.Interfaces;
using DesafioAssino.Application.Services;
using DesafioAssino.Domain.Entities;
using DesafioAssino.Domain.Exceptions;
using FluentAssertions;
using Moq;
using Xunit;

namespace DesafioAssino.Application.Tests;

public class ObterPendentesServiceTests
{
    private readonly Mock<ITarefaRepository> _repositoryMock = new();

    [Fact]
    public async Task Deve_retornar_lista_de_tarefas_pendentes(){
        var tarefas = new List<TarefaItem>{
            new("Tarefa 1", 2, "a.pdf"),
            new("Tarefa 2", 3, "b.pdf")
        };

        _repositoryMock
            .Setup(r => r.ObterPendentesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(tarefas);

        var service = new ObterPendentesService(_repositoryMock.Object);

        var result = await service.ExecutarAsync(CancellationToken.None);

        result.Should().NotBeNull();
        result.Should().HaveCount(2);
        result[0].Titulo.Should().Be("Tarefa 1");
    }

    [Fact]
    public async Task Deve_retornar_lista_vazia_se_nao_houver_pendentes(){
        _repositoryMock
            .Setup(r => r.ObterPendentesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<TarefaItem>());

        var service = new ObterPendentesService(_repositoryMock.Object);

        var result = await service.ExecutarAsync(CancellationToken.None);

        result.Should().NotBeNull();
        result.Should().BeEmpty();
    }

    [Fact]
    public async Task Deve_lancar_excecao_se_repositorio_retornar_null(){
        _repositoryMock
            .Setup(r => r.ObterPendentesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(((List<TarefaItem>?)null)!);

        var service = new ObterPendentesService(_repositoryMock.Object);

        Func<Task> act = async () => await service.ExecutarAsync(CancellationToken.None);

        await act.Should().ThrowAsync<DomainException>().WithMessage("Nenhuma tarefa pendente.");
    }
}
