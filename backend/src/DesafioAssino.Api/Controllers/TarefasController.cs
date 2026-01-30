using DesafioAssino.Api.Models;
using DesafioAssino.Application.DTOs;
using DesafioAssino.Application.Interfaces;
using DesafioAssino.Domain.Enums;
using Microsoft.AspNetCore.Mvc;

namespace DesafioAssino.Api.Controllers;

/// <summary>
/// Gerencia as tarefas do sistema.
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class TarefasController(ICriarTarefaService criarTarefaService, IListarTarefasService listarTarefasService, IConcluirTarefaService concluirTarefaService,
    IObterPendentesService obterPendentesService) : ControllerBase{

    /// <summary>
    /// Cria uma nova tarefa com upload de arquivo.
    /// </summary>
    /// <remarks>
    /// Exemplo de requisição:
    /// 
    ///     POST /api/tarefas
    ///     Content-Type: multipart/form-data
    ///     
    ///     Formulário:
    ///     - Titulo: "Minha Tarefa"
    ///     - SlaHoras: 24
    ///     - Arquivo: (binário do arquivo)
    /// </remarks>
    /// <param name="form">Dados da tarefa e arquivo.</param>
    /// <param name="cancellationToken">Token de cancelamento.</param>
    /// <returns>A tarefa criada.</returns>
    /// <response code="201">Tarefa criada com sucesso.</response>
    /// <response code="400">Dados inválidos ou arquivo ausente.</response>
    [HttpPost]
    [Consumes("multipart/form-data")]
    [ProducesResponseType(typeof(TarefaResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Criar([FromForm] CriarTarefaForm form, CancellationToken cancellationToken)
    {
        if (form.Arquivo == null || form.Arquivo.Length == 0)
            return BadRequest(new ErrorResponse { Message = "Arquivo é obrigatório." });

        using var memoryStream = new MemoryStream();
        await form.Arquivo.CopyToAsync(memoryStream, cancellationToken);

        var request = new CriarTarefaRequest
        {
            Titulo = form.Titulo,
            SlaHoras = form.SlaHoras,
            ArquivoNome = form.Arquivo.FileName,
            ArquivoConteudo = memoryStream.ToArray()
        };

        var response = await criarTarefaService.ExecutarAsync(request, cancellationToken);
        return CreatedAtAction(nameof(Listar), new { }, response);
    }

    /// <summary>
    /// Lista todas as tarefas, com opção de filtro por status.
    /// </summary>
    /// <param name="status">Filtro opcional de status (Pendente, Concluida, Expirada).</param>
    /// <param name="cancellationToken">Token de cancelamento.</param>
    /// <returns>Lista de tarefas.</returns>
    /// <response code="200">Lista retornada com sucesso.</response>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<TarefaResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Listar([FromQuery] EnumStatus? status, CancellationToken cancellationToken)
    {
        var tarefas = await listarTarefasService.ExecutarAsync(status, cancellationToken);
        return Ok(tarefas);
    }

    /// <summary>
    /// Obtém apenas as tarefas pendentes.
    /// </summary>
    /// <param name="cancellationToken">Token de cancelamento.</param>
    /// <returns>Lista de tarefas pendentes.</returns>
    /// <response code="200">Lista retornada com sucesso.</response>
    [HttpGet("pendentes")]
    [ProducesResponseType(typeof(IEnumerable<TarefaResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> ObterPendentes(CancellationToken cancellationToken)
    {
        var tarefas = await obterPendentesService.ExecutarAsync(cancellationToken);
        return Ok(tarefas);
    }

    /// <summary>
    /// Conclui uma tarefa existente.
    /// </summary>
    /// <param name="id">ID da tarefa.</param>
    /// <param name="cancellationToken">Token de cancelamento.</param>
    /// <returns>Sem conteúdo.</returns>
    /// <response code="204">Tarefa concluída com sucesso.</response>
    /// <response code="404">Tarefa não encontrada.</response>
    /// <response code="400">Não é possível concluir tarefa expirada.</response>
    [HttpPut("{id:guid}/concluir")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Concluir(Guid id, CancellationToken cancellationToken)
    {
        await concluirTarefaService.ExecutarAsync(id, cancellationToken);
        return NoContent();
    }
}
