namespace DesafioAssino.Api.Models;

public class CriarTarefaForm{
    public string Titulo { get; set; } = string.Empty;
    public int SlaHoras { get; set; }
    public IFormFile Arquivo { get; set; } = null!;
}
