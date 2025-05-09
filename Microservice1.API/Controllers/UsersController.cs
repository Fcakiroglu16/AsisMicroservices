using MassTransit;
using Microservice1.API.Services;
using Microsoft.AspNetCore.Mvc;
using ServiceBus.Shared;

namespace Microservice1.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UsersController(IPublishEndpoint publishEndpoint, StockServices stockServices) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var response = await stockServices.GetStockDataAsync();
        return Ok(response);
    }


    //user created
    [HttpPost]
    public async Task<IActionResult> Create()
    {
        // Simulate user creation
        //  count, timeout(best practice)

        var cancellationTokenSource = new CancellationTokenSource();
        cancellationTokenSource.CancelAfter(TimeSpan.FromSeconds(30));

        await publishEndpoint.Publish(new UserCreatedEvent("abc", "ahmet16", DateTime.Now), pipeline =>
        {
            pipeline.SetAwaitAck(true);
            pipeline.Headers.Set("version", "v1");
            pipeline.Headers.Set("correlation_id", Guid.NewGuid());
        }, cancellationTokenSource.Token);
        return Ok();
    }
}