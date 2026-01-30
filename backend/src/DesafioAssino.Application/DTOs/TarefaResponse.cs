using DesafioAssino.Domain.Enums;

namespace DesafioAssino.Application.DTOs;

public class TarefaResponse{
    public int NumTarefa { get; set; }
    public string Titulo { get; set; } = string.Empty;
    public EnumStatus Status { get; set; }
    public DateTime DataCriacao { get; set; }
    public DateTime? DataConclusao { get; set; }
    public DateTime DataExpiracao { get; set; }
}
