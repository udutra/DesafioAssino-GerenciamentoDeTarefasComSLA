using DesafioAssino.Domain.Entities;
using DesafioAssino.Domain.Exceptions;
using FluentAssertions;

namespace DesafioAssino.Domain.Tests.Entities;

public class TarefaItemTests{
    [Fact]
    public void Deve_criar_tarefa_valida(){
        var tarefa = new TarefaItem(titulo: "Tarefa Teste",slaHoras: 2,arquivoPath: "arquivo.pdf");

        tarefa.Id.Should().NotBeEmpty();
        tarefa.Titulo.Should().Be("Tarefa Teste");
        tarefa.SlaHoras.Should().Be(2);
        tarefa.Status.Should().Be(EnumStatus.Pendente);
        tarefa.DataCriacao.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(2));
    }

    [Fact]
    public void Nao_deve_criar_tarefa_com_titulo_vazio(){
        var act = () => new TarefaItem("", 2, "arquivo.pdf");

        act.Should()
            .Throw<DomainException>()
            .WithMessage("*Título*");
    }

    [Fact]
    public void Nao_deve_criar_tarefa_com_sla_invalido(){
        var act = () => new TarefaItem("Teste", 0, "arquivo.pdf");

        act.Should()
            .Throw<DomainException>()
            .WithMessage("*SLA*");
    }

    [Fact]
    public void Nao_deve_criar_tarefa_sem_arquivo(){
        var act = () => new TarefaItem("Teste", 1, "");

        act.Should()
            .Throw<DomainException>()
            .WithMessage("*Arquivo*");
    }

    [Fact]
    public void Deve_concluir_tarefa_pendente(){
        var tarefa = new TarefaItem("Teste", 1, "arquivo.pdf");

        tarefa.Concluir();

        tarefa.Status.Should().Be(EnumStatus.Concluida);
        tarefa.DataConclusao.Should().NotBeNull();
    }

    [Fact]
    public void Nao_deve_concluir_tarefa_expirada(){
        var tarefa = new TarefaItem("Teste", 1, "arquivo.pdf");

        tarefa.MarcarComoExpirada();

        var act = () => tarefa.Concluir();

        act.Should().Throw<DomainException>();
    }

    [Fact]
    public void Deve_identificar_tarefa_expirada(){
        var tarefa = new TarefaItem("Teste", 1, "arquivo.pdf");

        var agora = tarefa.DataCriacao.AddHours(2);

        var expirada = tarefa.EstaExpirada(agora);

        expirada.Should().BeTrue();
    }

    [Fact]
    public void Nao_deve_marcar_como_expirada_tarefa_concluida(){
        var tarefa = new TarefaItem("Teste", 1, "arquivo.pdf");
        tarefa.Concluir();

        var act = () => tarefa.MarcarComoExpirada();

        act.Should().Throw<DomainException>();
    }

    [Fact]
    public void Deve_marcar_tarefa_como_expirada(){
        var tarefa = new TarefaItem("Teste", 1, "arquivo.pdf");

        tarefa.MarcarComoExpirada();

        tarefa.Status.Should().Be(EnumStatus.Expirada);
    }
}