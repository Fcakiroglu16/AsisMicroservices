using MassTransit;
using ServiceBus.Shared;

namespace Microservice2.API.Consumers
{
    public class UserCreatedEventConsumer : IConsumer<UserCreatedEvent>
    {
        public async Task Consume(ConsumeContext<UserCreatedEvent> context)
        {
            var userCreatedEvent = context.Message;

            Console.WriteLine($"User Created: {userCreatedEvent.UserId}, {userCreatedEvent.UserName}, {userCreatedEvent.CreatedAt}");
        }
    }
}
