using MassTransit;
using Microservice1.API.Options;
using Microservice1.API.Services;
using Polly;
using Polly.Extensions.Http;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();


var rabbitMqOptions = builder.Configuration.GetSection(nameof(RabbitMqOption)).Get<RabbitMqOption>();

//add masstransit
builder.Services.AddMassTransit(x =>
{
    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host($"{rabbitMqOptions!.Host}", hostConfiguration =>
        {
            hostConfiguration.Username(rabbitMqOptions.UserName);
            hostConfiguration.Password(rabbitMqOptions.Password);
        });
    });
});

// Polly politikasını yapılandırıyoruz
var retryPolicy = HttpPolicyExtensions
    .HandleTransientHttpError() // HTTP 5xx, 408 ve network hatalarını yakalar
    /*.OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.NotFound)*/ // 404 hatalarını da yakalar
    .WaitAndRetryAsync(3,
        retryAttempt => TimeSpan.FromSeconds(Math.Pow(1, retryAttempt))); // 3 kez dene, aradaki süreyi arttır


var circuitBreakerPolicy = HttpPolicyExtensions
    .HandleTransientHttpError()
    .CircuitBreakerAsync(2, TimeSpan.FromMinutes(1)); // 2 hata sonrası 1 dakika devreye girmez


//advanced circuit  breaker

var circuitAdvancedBreakerPolicy = HttpPolicyExtensions
    .HandleTransientHttpError().AdvancedCircuitBreakerAsync(0.8, TimeSpan.FromMinutes(1), 3, TimeSpan.FromSeconds(30));


//timeout
var timeoutPolicy = Policy.TimeoutAsync<HttpResponseMessage>(5);


var combinedPolicy = Policy.WrapAsync(retryPolicy, circuitAdvancedBreakerPolicy, timeoutPolicy);

builder.Services.AddHttpClient<StockServices>(x => { x.BaseAddress = new Uri(builder.Configuration.GetSection("Microservices")["Microservice2BaseUrl"]!); }).AddPolicyHandler(combinedPolicy);


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseSwagger();
app.UseSwaggerUI();
app.UseAuthorization();

app.MapControllers();

app.Run();
