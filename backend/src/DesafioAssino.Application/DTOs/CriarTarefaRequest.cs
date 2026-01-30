namespace DesafioAssino.Application.DTOs;

public class CriarTarefaRequest{
    public string Titulo { get; set; } = string.Empty;
    public int SlaHoras { get; set; }
    public string ArquivoNome { get; set; } = string.Empty;
    public byte[] ArquivoConteudo { get; set; } = [];
}