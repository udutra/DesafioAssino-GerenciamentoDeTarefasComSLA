using DesafioAssino.Application.Interfaces;
using DesafioAssino.Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc;

namespace DesafioAssino.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TestingController(AppDbContext context, IObterPorNumTarefaService obterPorNumTarefaService, IExpirarSlaService expirarSlaService) : ControllerBase
{
    [HttpPost("retroagir-tarefa/{numTarefa:int}")]
    public async Task<IActionResult> RetroagirDataCriacao(int numTarefa, [FromQuery] int horas, CancellationToken cancellationToken)
    {
        var tarefa = await obterPorNumTarefaService.ExecutarAsync(numTarefa, cancellationToken);

        context.Entry(tarefa).Property(t => t.DataCriacao).CurrentValue = DateTime.UtcNow.AddHours(-horas);
        await context.SaveChangesAsync(cancellationToken);

        return Ok(new { Message = $"Tarefa retroagida em {horas} horas.", NovaDataCriacao = tarefa.DataCriacao, tarefa.DataExpiracao });
    }

    [HttpPost("rodar-job-expiracao")]
    public async Task<IActionResult> RodarJobExpiracao(CancellationToken cancellationToken){
        await expirarSlaService.ExpirarAsync(cancellationToken);
        return Ok("Job de expiração executado com sucesso.");
    }
}
