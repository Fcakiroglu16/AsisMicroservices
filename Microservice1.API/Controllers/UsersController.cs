using MassTransit;
using Microsoft.AspNetCore.Mvc;
using ServiceBus.Shared;

namespace Microservice1.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController(IPublishEndpoint publishEndpoint) : ControllerBase
    {
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
                pipeline.Headers.Set("version", "v1");
                pipeline.Headers.Set("correlation_id", Guid.NewGuid());
            }, cancellationTokenSource.Token);
            return Ok();
        }
    }
}
