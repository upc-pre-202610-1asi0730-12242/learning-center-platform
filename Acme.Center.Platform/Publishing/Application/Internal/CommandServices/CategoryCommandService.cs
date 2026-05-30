using Acme.Center.Platform.Publishing.Domain.Model.Commands;
using Acme.Center.Platform.Publishing.Domain.Model.Entities;
using Acme.Center.Platform.Publishing.Domain.Model.Events;
using Acme.Center.Platform.Publishing.Domain.Repositories;
using Acme.Center.Platform.Publishing.Domain.Services;
using Acme.Center.Platform.Shared.Domain.Repositories;
using Cortex.Mediator;

namespace Acme.Center.Platform.Publishing.Application.Internal.CommandServices;

/// <summary>
///     Represents the category command service in the ACME Learning Center Platform.
/// </summary>
/// <param name="categoryRepository">
///     The <see cref="ICategoryRepository" /> to use.
/// </param>
/// <param name="unitOfWork">
///     The <see cref="IUnitOfWork" /> to use.
/// </param>
/// <param name="domainEventPublisher">
///   The <see cref="IMediator" /> to use for publishing domain events.
/// </param>
public class CategoryCommandService(ICategoryRepository categoryRepository, IUnitOfWork unitOfWork, IMediator domainEventPublisher)
    : ICategoryCommandService
{
    /// <inheritdoc />
    public async Task<Category?> Handle(CreateCategoryCommand command)
    {
        var category = new Category(command);
        await categoryRepository.AddAsync(category);
        await unitOfWork.CompleteAsync();
        
        // Publish the domain event after the category is created
        await domainEventPublisher.PublishAsync(new CategoryCreatedEvent(category.Name));
        
        // Return the created category
        return category;
    }
}