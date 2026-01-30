using DesafioAssino.Domain.Entities;
using DesafioAssino.Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DesafioAssino.Integration.Tests;

public class ApiTestFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(Microsoft.AspNetCore.Hosting.IWebHostBuilder builder)
    {
        builder.ConfigureAppConfiguration((context, config) => {
            config.AddUserSecrets(typeof(Program).Assembly, optional: false);
        });

        builder.ConfigureServices((context, services) =>
        {
            var configuration = context.Configuration;

            var connectionString = configuration.GetConnectionString("DefaultConnectionTest");
            if (string.IsNullOrEmpty(connectionString))
                throw new InvalidOperationException("Connection string 'DefaultConnectionTest' não encontrada nos user-secrets!");

            var descriptors = services
                .Where(d => d.ServiceType == typeof(DbContextOptions<AppDbContext>) ||
                            d.ImplementationType == typeof(AppDbContext))
                .ToList();
            foreach (var descriptor in descriptors)
                services.Remove(descriptor);

            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlServer(connectionString);
            });

            var sp = services.BuildServiceProvider();
            using var scope = sp.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            db.Database.EnsureCreated();
            SeedDatabase(db);
        });
    }

    private void SeedDatabase(AppDbContext db){
        db.Tarefas.RemoveRange(db.Tarefas);
        db.SaveChanges();

        var tarefa = new TarefaItem("Tarefa Inicial", 2, "a.pdf");
        db.Tarefas.Add(tarefa);
        db.SaveChanges();
    }

}