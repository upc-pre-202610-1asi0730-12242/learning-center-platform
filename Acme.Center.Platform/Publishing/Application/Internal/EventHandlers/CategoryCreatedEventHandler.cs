using Acme.Center.Platform.Publishing.Domain.Model.Events;
using Acme.Center.Platform.Shared.Application.Internal.EventHandlers;

namespace Acme.Center.Platform.Publishing.Application.Internal.EventHandlers;

public class CategoryCreatedEventHandler : IEventHandler<CategoryCreatedEvent>
{
    public Task Handle(CategoryCreatedEvent domainEvent, CancellationToken cancellationToken)
    {
        return On(domainEvent);
    }
    
    private static Task On(CategoryCreatedEvent domainEvent) {
        Console.WriteLine("Created Category: {0}",domainEvent.Name);
        return Task.CompletedTask;    
    }
}