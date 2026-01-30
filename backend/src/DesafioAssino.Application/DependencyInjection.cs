using DesafioAssino.Application.Interfaces;
using DesafioAssino.Application.Services;
using DesafioAssino.Application.Validators;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace DesafioAssino.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<ICriarTarefaService, CriarTarefaService>();
        services.AddScoped<IConcluirTarefaService, ConcluirTarefaService>();
        services.AddScoped<IExpirarSlaService, ExpirarSlaService>();
        services.AddScoped<IListarTarefasService, ListarTarefasService>();
        services.AddScoped<IObterPendentesService, ObterPendentesService>();
        services.AddScoped<IObterPorNumTarefaService, ObterPorNumTarefaService>();

        services.AddValidatorsFromAssemblyContaining<CriarTarefaRequestValidator>();

        return services;
    }
}
