using DesafioAssino.Application.DTOs;
using DesafioAssino.Application.Interfaces;
using DesafioAssino.Application.Services;
using DesafioAssino.Application.Validators;
using DesafioAssino.Domain.Entities;
using DesafioAssino.Domain.Enums;
using FluentAssertions;
using FluentValidation;
using Moq;
using Xunit;

namespace DesafioAssino.Application.Tests;

public class CriarTarefaServiceTests{
    private readonly Mock<ITarefaRepository> _repositoryMock = new();
    private readonly Mock<IFileStorageService> _fileStorageMock = new();
    private readonly IValidator<CriarTarefaRequest> _validator = new CriarTarefaRequestValidator();

    [Fact]
    public async Task Deve_criar_tarefa_com_sucesso(){
        var service = new CriarTarefaService(_repositoryMock.Object, _fileStorageMock.Object, _validator);

        _fileStorageMock.Setup(x => x.SalvarAsync(It.IsAny<string>(), It.IsAny<byte[]>(), It.IsAny<CancellationToken>())).ReturnsAsync("path/arquivo.pdf");

        var request = new CriarTarefaRequest{
            Titulo = "Nova Tarefa",
            SlaHoras = 2,
            ArquivoNome = "arquivo.pdf",
            ArquivoConteudo = [1, 2, 3]
        };

        var response = await service.ExecutarAsync(request, CancellationToken.None);

        response.Titulo.Should().Be("Nova Tarefa");
        response.Status.Should().Be(EnumStatus.Pendente);
        response.DataCriacao.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));

        _repositoryMock.Verify(
            x => x.AddAsync(It.IsAny<TarefaItem>(), It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task Deve_lancar_excecao_quando_request_invalido(){
        var service = new CriarTarefaService(_repositoryMock.Object, _fileStorageMock.Object, _validator);

        var request = new CriarTarefaRequest();

        Func<Task> act = async () => await service.ExecutarAsync(request, CancellationToken.None);

        await act.Should().ThrowAsync<ValidationException>();
    }
}