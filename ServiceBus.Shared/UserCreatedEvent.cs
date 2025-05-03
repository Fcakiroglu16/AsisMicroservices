namespace ServiceBus.Shared
{
    public record UserCreatedEvent(string UserId, string UserName, DateTime CreatedAt);
}
