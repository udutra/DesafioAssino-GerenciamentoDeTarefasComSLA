using DesafioAssino.Application.DTOs;
using FluentValidation;

namespace DesafioAssino.Application.Validators;

public class CriarTarefaRequestValidator
    : AbstractValidator<CriarTarefaRequest>{
    public CriarTarefaRequestValidator(){
        RuleFor(x => x.Titulo)
            .NotEmpty();

        RuleFor(x => x.SlaHoras)
            .GreaterThan(0);

        RuleFor(x => x.ArquivoConteudo)
            .NotEmpty();

        RuleFor(x => x.ArquivoNome)
            .NotEmpty();
    }
}