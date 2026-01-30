using DesafioAssino.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DesafioAssino.Infrastructure.Persistence;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options){
    public DbSet<TarefaItem> Tarefas => Set<TarefaItem>();

    protected override void OnModelCreating(ModelBuilder modelBuilder){
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}