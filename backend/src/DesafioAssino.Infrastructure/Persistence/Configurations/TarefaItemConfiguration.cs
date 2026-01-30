using DesafioAssino.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DesafioAssino.Infrastructure.Persistence.Configurations;

public class TarefaItemConfiguration : IEntityTypeConfiguration<TarefaItem>{
    public void Configure(EntityTypeBuilder<TarefaItem> builder){
        builder.ToTable("Tarefas");

        builder.HasKey(t => t.Id);

        builder.Property(t => t.Id)
            .ValueGeneratedNever();

        builder.Property(t => t.NumTarefa)
            .ValueGeneratedOnAdd()
            .UseIdentityColumn();

        builder.HasIndex(t => t.NumTarefa)
            .IsUnique();

        builder.Property(t => t.Titulo)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(t => t.SlaHoras)
            .IsRequired();

        builder.Property(t => t.ArquivoPath)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(t => t.DataCriacao)
            .IsRequired();

        builder.Property(t => t.DataConclusao)
            .IsRequired(false);

        builder.Property(t => t.Status)
            .IsRequired()
            .HasConversion<int>();

        builder.Ignore(t => t.DataExpiracao);

        builder.HasIndex(t => new { t.Status, t.DataCriacao });
    }
}