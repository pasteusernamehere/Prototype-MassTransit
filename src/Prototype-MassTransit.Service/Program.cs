// See https://aka.ms/new-console-template for more information

using System.Diagnostics;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Prototype_MassTransit.Components.Reserves.Consumers;
using Prototype_MassTransit.Components.Reserves.StateMachines;

var isService = !(Debugger.IsAttached || args.Contains("--console"));

var builder = new HostBuilder().ConfigureAppConfiguration((hostingContext, config) =>
    {
        config.AddJsonFile("appsettings.json", true);
        config.AddEnvironmentVariables();

        if (args != null)
        {
            config.AddCommandLine(args);
        }
    })
    .ConfigureServices((hostingContext, services) =>
    {
        services.TryAddSingleton(KebabCaseEndpointNameFormatter.Instance);

        services.AddMassTransit(x =>
        {
            x.SetKebabCaseEndpointNameFormatter();

            x.AddSagaStateMachine<ReserveStateMachine, ReserveState>(cfg =>
                {
                    cfg.UseDelayedRedelivery(r =>
                        r.Intervals(TimeSpan.FromMinutes(5), TimeSpan.FromMinutes(15), TimeSpan.FromMinutes(30)));
                    cfg.UseMessageRetry(r => r.Intervals(100, 200, 500, 800, 1000));
                    cfg.UseInMemoryOutbox();
                })
                .RedisRepository("127.0.0.1");

            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.UseDelayedRedelivery(r =>
                    r.Intervals(TimeSpan.FromMinutes(5), TimeSpan.FromMinutes(15), TimeSpan.FromMinutes(30)));
                cfg.UseMessageRetry(r => r.Intervals(100, 200, 500, 800, 1000));
                cfg.UseInMemoryOutbox();

                cfg.Host("localhost", "/", h =>
                {
                    h.Username("guest");
                    h.Password("guest");
                });

                cfg.ConfigureEndpoints(context);
            });

            x.AddConsumersFromNamespaceContaining<SubmitReserveConsumer>();
        });
        services.AddHostedService<MassTransitHostedService>();
    })
    .ConfigureLogging((hostingContext, logging) =>
    {
        logging.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
        logging.AddConsole();
    });


if (isService)
{
    await builder.UseWindowsService()
        .Build()
        .RunAsync();
}
else
{
    await builder.RunConsoleAsync();
}