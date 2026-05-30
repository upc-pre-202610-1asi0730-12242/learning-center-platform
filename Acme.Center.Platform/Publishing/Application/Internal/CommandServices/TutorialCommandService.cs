using Acme.Center.Platform.Publishing.Application.CommandServices;
using Acme.Center.Platform.Publishing.Domain.Model.Aggregate;
using Acme.Center.Platform.Publishing.Domain.Model.Commands;
using Acme.Center.Platform.Publishing.Domain.Repositories;
using Acme.Center.Platform.Shared.Application.Model;
using Acme.Center.Platform.Shared.Domain.Repositories;

namespace Acme.Center.Platform.Publishing.Application.Internal.CommandServices;

public class TutorialCommandService(
    ITutorialRepository tutorialRepository,
    ICategoryRepository categoryRepository,
    IUnitOfWork unitOfWork)
    : ITutorialCommandService
{
    /// <inheritdoc />
    public async Task<Result<Tutorial>> Handle(AddVideoAssetToTutorialCommand command)
    {
        var tutorial = await tutorialRepository.FindByIdAsync(command.TutorialId);
        if (tutorial is null) return Result<Tutorial>.Failure("Tutorial not found");
        tutorial.AddVideo(command.VideoUrl);
        await unitOfWork.CompleteAsync();
        return Result<Tutorial>.Success(tutorial);
    }

    /// <inheritdoc />
    public async Task<Result<Tutorial>> Handle(CreateTutorialCommand command)
    {
        var category = await categoryRepository.FindByIdAsync(command.CategoryId);
        if (category is null) return Result<Tutorial>.Failure("Category not found");
        if (await tutorialRepository.ExistsByTitleAsync(command.Title))
            return Result<Tutorial>.Failure("Tutorial with the same title already exists");
        var tutorial = new Tutorial(command);
        await tutorialRepository.AddAsync(tutorial);
        await unitOfWork.CompleteAsync();
        tutorial.Category = category;
        return Result<Tutorial>.Success(tutorial);
    }
}