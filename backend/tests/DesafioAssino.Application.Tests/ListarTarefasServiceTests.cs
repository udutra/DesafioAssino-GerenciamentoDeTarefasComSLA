using DesafioAssino.Application.Interfaces;
using DesafioAssino.Application.Services;
using DesafioAssino.Domain.Entities;
using DesafioAssino.Domain.Enums;
using FluentAssertions;
using Moq;
using Xunit;

namespace DesafioAssino.Application.Tests;

public class ListarTarefasServiceTests{
    private readonly Mock<ITarefaRepository> _repositoryMock = new();

    [Fact]
    public async Task Deve_listar_todas_as_tarefas_e_mapear_para_dto()
    {
        var tarefa1 = new TarefaItem("Tarefa 1", 2, "a.pdf");
        var tarefa2 = new TarefaItem("Tarefa 2", 3, "b.pdf");
        
        var tarefas = new List<TarefaItem>{ tarefa1, tarefa2 };

        _repositoryMock
            .Setup(r => r.ListarAsync(null, It.IsAny<CancellationToken>()))
            .ReturnsAsync(tarefas);

        var service = new ListarTarefasService(_repositoryMock.Object);

        var result = await service.ExecutarAsync(null, CancellationToken.None);

        result.Should().HaveCount(2);
        
        result[0].Titulo.Should().Be("Tarefa 1");
        result[0].NumTarefa.Should().Be(tarefa1.NumTarefa);
        result[0].Status.Should().Be(EnumStatus.Pendente);
        
        result[1].Titulo.Should().Be("Tarefa 2");
    }


    [Fact]
    public async Task Deve_listar_apenas_tarefas_concluidas(){
        var tarefaConcluida = new TarefaItem("Tarefa Concluída", 2, "a.pdf");
        tarefaConcluida.Concluir();

        var tarefas = new List<TarefaItem>{ tarefaConcluida };

        _repositoryMock
            .Setup(r => r.ListarAsync(EnumStatus.Concluida, It.IsAny<CancellationToken>()))
            .ReturnsAsync(tarefas);

        var service = new ListarTarefasService(_repositoryMock.Object);

        var result = await service.ExecutarAsync(EnumStatus.Concluida, CancellationToken.None);

        result.Should().HaveCount(1);
        result[0].Status.Should().Be(EnumStatus.Concluida);
        result[0].DataConclusao.Should().NotBeNull();
    }
}