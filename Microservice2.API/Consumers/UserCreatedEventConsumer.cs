using MassTransit;
using ServiceBus.Shared;

namespace Microservice2.API.Consumers
{
    public class UserCreatedEventConsumer : IConsumer<UserCreatedEvent>
    {
        public Task Consume(ConsumeContext<UserCreatedEvent> context)
        {
            Console.WriteLine("UserCreatedEventConsumer consumer çalıştı");


            throw new Exception("Db hatası");

            // Handle the event here
            var userCreatedEvent = context.Message;

            Console.WriteLine($"User Created: {userCreatedEvent.UserId}, {userCreatedEvent.UserName}, {userCreatedEvent.CreatedAt}");
            return Task.CompletedTask;
        }
    }
}
