using MassTransit;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Prototype_MassTransit.Api;
using Prototype_MassTransit.Components.Reserves.Consumers;
using Prototype_MassTransit.Contracts.Reserves;

var builder = WebApplication.CreateBuilder(args)
    .ConfigureServices(services =>
    {
        services.TryAddSingleton(KebabCaseEndpointNameFormatter.Instance);

        services.AddMassTransit(x =>
        {
            x.SetKebabCaseEndpointNameFormatter();

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

            x.AddConsumer<SubmitReserveConsumer>();
            x.AddRequestClient<ISubmitReserve>();
            x.AddRequestClient<ISubmittedReserve>();
        });
    });

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();