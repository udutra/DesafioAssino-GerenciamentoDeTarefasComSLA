using DesafioAssino.Application.Interfaces;
using DesafioAssino.Infrastructure.Persistence;
using DesafioAssino.Infrastructure.Repositories;
using DesafioAssino.Infrastructure.Services;
using DesafioAssino.Infrastructure.Storage;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DesafioAssino.Infrastructure;

public static class DependencyInjection{

    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration, bool isTesting = false){
        if (!isTesting){
            services.AddDbContext<AppDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
        }

        services.AddScoped<ITarefaRepository, TarefaRepository>();
        services.AddScoped<IFileStorageService, LocalFileStorageService>();
        services.AddScoped<INotificacaoService, LogNotificacaoService>();

        return services;
    }
}