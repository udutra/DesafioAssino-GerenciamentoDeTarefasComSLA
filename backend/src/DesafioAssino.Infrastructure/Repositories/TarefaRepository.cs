using DesafioAssino.Application.Interfaces;
using DesafioAssino.Domain;
using DesafioAssino.Domain.Entities;
using DesafioAssino.Domain.Enums;
using DesafioAssino.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace DesafioAssino.Infrastructure.Repositories;

public sealed class TarefaRepository(AppDbContext context) : ITarefaRepository{
    public async Task AddAsync(TarefaItem tarefa, CancellationToken cancellationToken){
        await context.Tarefas.AddAsync(tarefa, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<TarefaItem>> ListarAsync(EnumStatus? status, CancellationToken cancellationToken){
        var query = context.Tarefas.AsNoTracking();

        if (status.HasValue)
            query = query.Where(t => t.Status == status.Value);

        return await query
            .OrderByDescending(t => t.DataCriacao)
            .ToListAsync(cancellationToken: cancellationToken);
    }

    public async Task<IReadOnlyList<TarefaItem>> ObterPendentesAsync(CancellationToken cancellationToken){
        return await context.Tarefas
            .Where(t => t.Status == EnumStatus.Pendente)
            .ToListAsync(cancellationToken);
    }

    public async Task<List<TarefaItem>> ObterTarefasComSlaVencidoAsync(DateTime agora, CancellationToken cancellationToken){
        return await context.Tarefas
            .Where(t =>
                t.Status == EnumStatus.Pendente &&
                t.DataCriacao.AddHours(t.SlaHoras) < agora)
            .ToListAsync(cancellationToken);
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken){
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task<TarefaItem?> ObterPorIdAsync(Guid id, CancellationToken cancellationToken){
        return await context.Tarefas.SingleOrDefaultAsync(t => t.Id == id, cancellationToken: cancellationToken);
    }

    public async Task<TarefaItem?> ObterPorNumTarefaAsync(int numTarefa, CancellationToken cancellationToken){
        return await context.Tarefas.SingleOrDefaultAsync(t => t.NumTarefa == numTarefa, cancellationToken: cancellationToken);
    }
}