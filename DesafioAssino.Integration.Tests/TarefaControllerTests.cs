using System.Net;
using System.Net.Http.Json;
using DesafioAssino.Application.DTOs;
using DesafioAssino.Domain.Enums;
using FluentAssertions;

namespace DesafioAssino.Integration.Tests;

public class TarefaControllerTests : IClassFixture<ApiTestFactory>
{
    private readonly HttpClient _client;

    public TarefaControllerTests(ApiTestFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Criar_Tarefa_Retorna_Tarefa()
    {
        var content = new MultipartFormDataContent();
        content.Add(new StringContent("Nova Tarefa"), "Titulo");
        content.Add(new StringContent("2"), "SlaHoras");

        var fileContent = new ByteArrayContent(new byte[] {1, 2, 3});
        fileContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/octet-stream");
        content.Add(fileContent, "Arquivo", "arquivo.pdf");

        var response = await _client.PostAsync("/api/tarefas", content);

        response.StatusCode.Should().Be(HttpStatusCode.Created);

        var json = await response.Content.ReadAsStringAsync();

        var options = new System.Text.Json.JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            Converters = { new System.Text.Json.Serialization.JsonStringEnumConverter() }
        };

        var tarefa = System.Text.Json.JsonSerializer.Deserialize<TarefaResponse>(json, options);

        tarefa!.NumTarefa.Should().BeGreaterThan(0);
        tarefa.Titulo.Should().Be("Nova Tarefa");
    }

    [Fact]
    public async Task Listar_Tarefas_SemStatus_RetornaTodas(){
        var response = await _client.GetAsync("/api/tarefas");
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var json = await response.Content.ReadAsStringAsync();
        var options = new System.Text.Json.JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            Converters = { new System.Text.Json.Serialization.JsonStringEnumConverter() }
        };
        var tarefas = System.Text.Json.JsonSerializer.Deserialize<List<TarefaResponse>>(json, options);

        tarefas.Should().NotBeNull();
        tarefas!.Count.Should().BeGreaterThan(0);
    }

    [Fact]
    public async Task Listar_Tarefas_ComStatus_RetornaApenasStatusEspecifico(){
        var status = EnumStatus.Pendente;

        var response = await _client.GetAsync($"/api/tarefas?status={status}");
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var json = await response.Content.ReadAsStringAsync();
        var options = new System.Text.Json.JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            Converters = { new System.Text.Json.Serialization.JsonStringEnumConverter() }
        };
        var tarefas = System.Text.Json.JsonSerializer.Deserialize<List<TarefaResponse>>(json, options);

        tarefas.Should().NotBeNull();
        tarefas.All(t => t.Status == status).Should().BeTrue();
    }
}