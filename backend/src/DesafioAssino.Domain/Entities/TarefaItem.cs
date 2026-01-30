using DesafioAssino.Domain.Enums;
using DesafioAssino.Domain.Exceptions;

namespace DesafioAssino.Domain.Entities;

public class TarefaItem{
    public Guid Id { get; private set; }
    public int NumTarefa { get; private set; }
    public string Titulo { get; private set; }
    public int SlaHoras { get; private set; }
    public DateTime DataCriacao { get; private set; }
    public DateTime? DataConclusao { get; private set; }
    public string ArquivoPath { get; private set; }
    public EnumStatus Status { get; private set; }
    public DateTime DataExpiracao => DataCriacao.AddHours(SlaHoras);

    public TarefaItem(string titulo, int slaHoras, string arquivoPath){
        if (string.IsNullOrWhiteSpace(titulo))
            throw new DomainException("Título é obrigatório.");

        if (slaHoras <= 0)
            throw new DomainException("SLA deve ser maior que zero.");

        if (string.IsNullOrWhiteSpace(arquivoPath))
            throw new DomainException("Arquivo é obrigatório.");

        Id = Guid.CreateVersion7();
        Titulo = titulo;
        SlaHoras = slaHoras;
        ArquivoPath = arquivoPath;
        DataCriacao = DateTime.UtcNow;
        Status = EnumStatus.Pendente;
    }

    public void Concluir(){
        if (Status == EnumStatus.Expirada)
            throw new DomainException("Não é possível concluir uma tarefa expirada.");

        Status = EnumStatus.Concluida;
        DataConclusao = DateTime.UtcNow;
    }

    public bool EstaExpirada(DateTime agora){
        if (Status == EnumStatus.Concluida)
            return false;

        return agora >= DataExpiracao;
    }

    public void MarcarComoExpirada(){
        if (Status == EnumStatus.Concluida)
            throw new DomainException("Não é possível expirar uma tarefa concluída.");

        Status = EnumStatus.Expirada;
    }
}