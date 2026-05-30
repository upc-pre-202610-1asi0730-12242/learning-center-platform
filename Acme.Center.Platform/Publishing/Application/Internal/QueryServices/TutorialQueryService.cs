using Acme.Center.Platform.Publishing.Domain.Model.Aggregate;
using Acme.Center.Platform.Publishing.Domain.Model.Queries;
using Acme.Center.Platform.Publishing.Domain.Repositories;
using Acme.Center.Platform.Publishing.Domain.Services;

namespace Acme.Center.Platform.Publishing.Application.Internal.QueryServices;

/// <summary>
///     Tutorial query service
/// </summary>
/// <param name="tutorialRepository">
///     Tutorial repository
/// </param>
public class TutorialQueryService(ITutorialRepository tutorialRepository) : ITutorialQueryService

{
    /// <inheritdoc />
    public async Task<Tutorial?> Handle(GetTutorialByIdQuery query)
    {
        return await tutorialRepository.FindByIdAsync(query.TutorialId);
    }

    /// <inheritdoc />
    public async Task<IEnumerable<Tutorial>> Handle(GetAllTutorialsQuery query)
    {
        return await tutorialRepository.ListAsync();
    }

    /// <inheritdoc />
    public async Task<IEnumerable<Tutorial>> Handle(GetAllTutorialsByCategoryIdQuery query)
    {
        return await tutorialRepository.FindByCategoryIdAsync(query.CategoryId);
    }
}