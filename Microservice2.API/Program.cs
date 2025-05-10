using MassTransit;
using Microservice2.API.Consumers;
using Microservice2.API.Options;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
//add masstransit


builder.Services.AddMassTransit(x =>
{
    var rabbitMqOptions = builder.Configuration.GetSection(nameof(RabbitMqOption)).Get<RabbitMqOption>();
    x.AddConsumer<UserCreatedEventConsumer>();

    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.UseConcurrencyLimit(5);

        cfg.PrefetchCount = 10;
        cfg.UseMessageRetry(r => r.Interval(3, TimeSpan.FromSeconds(5)));


        cfg.UseDelayedRedelivery(r => r.Interval(3, TimeSpan.FromMinutes(10)));

        cfg.Host($"{rabbitMqOptions!.Host}", hostConfiguration =>
        {
            hostConfiguration.Username(rabbitMqOptions.UserName);
            hostConfiguration.Password(rabbitMqOptions.Password);
        });


        // register consumer
        cfg.ReceiveEndpoint("microservice2.order-created.event", e => { e.ConfigureConsumer<UserCreatedEventConsumer>(context); });
    });
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
