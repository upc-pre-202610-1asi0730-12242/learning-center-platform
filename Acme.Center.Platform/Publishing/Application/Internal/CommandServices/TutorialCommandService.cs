using Acme.Center.Platform.Publishing.Domain.Model.Aggregate;
using Acme.Center.Platform.Publishing.Domain.Model.Commands;
using Acme.Center.Platform.Publishing.Domain.Repositories;
using Acme.Center.Platform.Publishing.Domain.Services;
using Acme.Center.Platform.Shared.Domain.Repositories;

namespace Acme.Center.Platform.Publishing.Application.Internal.CommandServices;

public class TutorialCommandService(
    ITutorialRepository tutorialRepository,
    ICategoryRepository categoryRepository,
    IUnitOfWork unitOfWork)
    : ITutorialCommandService
{
    /// <inheritdoc />
    public async Task<Tutorial?> Handle(AddVideoAssetToTutorialCommand command)
    {
        var tutorial = await tutorialRepository.FindByIdAsync(command.TutorialId);
        if (tutorial is null) throw new Exception("Tutorial not found");
        tutorial.AddVideo(command.VideoUrl);
        await unitOfWork.CompleteAsync();
        return tutorial;
    }

    /// <inheritdoc />
    public async Task<Tutorial?> Handle(CreateTutorialCommand command)
    {
        var category = await categoryRepository.FindByIdAsync(command.CategoryId);
        if (category is null) throw new Exception("Category not found");
        if (await tutorialRepository.ExistsByTitleAsync(command.Title))
            throw new Exception("Tutorial with the same title already exists");
        var tutorial = new Tutorial(command);
        await tutorialRepository.AddAsync(tutorial);
        await unitOfWork.CompleteAsync();
        tutorial.Category = category;
        return tutorial;
    }
}