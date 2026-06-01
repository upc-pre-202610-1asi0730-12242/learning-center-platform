using System.Net.Mime;
using Acme.Center.Platform.Publishing.Application.CommandServices;
using Acme.Center.Platform.Publishing.Application.QueryServices;
using Acme.Center.Platform.Publishing.Domain.Model.Queries;
using Acme.Center.Platform.Publishing.Interfaces.Rest.Resources;
using Acme.Center.Platform.Publishing.Interfaces.Rest.Transform;
using Acme.Center.Platform.Resources.Errors;
using Acme.Center.Platform.Shared.Interfaces.Rest.ProblemDetails;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Swashbuckle.AspNetCore.Annotations;
// Corrected using directive
// For ProblemDetailsFactory

// For PublishingError enum

namespace Acme.Center.Platform.Publishing.Interfaces.Rest;

[ApiController]
[Route("api/v1/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
[SwaggerTag("Available Tutorial endpoints")]
public class TutorialsController(
    ITutorialQueryService tutorialQueryService,
    ITutorialCommandService tutorialCommandService,
    IStringLocalizer<ErrorMessages> errorLocalizer, // Renamed for clarity
    ProblemDetailsFactory problemDetailsFactory) // Inject ProblemDetailsFactory
    : ControllerBase
{
    private readonly IStringLocalizer<ErrorMessages> _errorLocalizer = errorLocalizer;
    private readonly ProblemDetailsFactory _problemDetailsFactory = problemDetailsFactory;

    [HttpGet("{tutorialId:int}")]
    [SwaggerOperation(
        Summary = "Get a tutorial by its id",
        Description = "Get a tutorial by its id",
        OperationId = "GetTutorialById")]
    [SwaggerResponse(StatusCodes.Status200OK, "The tutorial was found", typeof(TutorialResource))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "The tutorial was not found")]
    public async Task<IActionResult> GetTutorialById([FromRoute] int tutorialId, CancellationToken cancellationToken)
    {
        var getTutorialByIdQuery = new GetTutorialByIdQuery(tutorialId);
        var tutorial = await tutorialQueryService.Handle(getTutorialByIdQuery, cancellationToken);

        return PublishingActionResultAssembler.ToActionResultFromGetTutorialByIdResult(
            this,
            tutorial,
            _errorLocalizer,
            _problemDetailsFactory,
            foundTutorial => Ok(TutorialResourceFromEntityAssembler.ToResourceFromEntity(foundTutorial))
        );
    }

    [HttpPost]
    [SwaggerOperation(
        Summary = "Create a tutorial",
        Description = "Create a tutorial",
        OperationId = "CreateTutorial")]
    [SwaggerResponse(StatusCodes.Status201Created, "The tutorial was created", typeof(TutorialResource))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "The tutorial was not created")]
    public async Task<IActionResult> CreateTutorial([FromBody] CreateTutorialResource resource,
        CancellationToken cancellationToken)
    {
        var createTutorialCommand = CreateTutorialCommandFromResourceAssembler.ToCommandFromResource(resource);
        var result = await tutorialCommandService.Handle(createTutorialCommand, cancellationToken);

        return PublishingActionResultAssembler.ToActionResultFromCreateTutorialResult(
            this,
            result,
            _errorLocalizer,
            _problemDetailsFactory,
            createdTutorial => CreatedAtAction(nameof(GetTutorialById), new { tutorialId = createdTutorial.Id },
                TutorialResourceFromEntityAssembler.ToResourceFromEntity(createdTutorial))
        );
    }

    [HttpGet]
    [SwaggerOperation(
        Summary = "Get all tutorials",
        Description = "Get all tutorials",
        OperationId = "GetAllTutorials")]
    [SwaggerResponse(StatusCodes.Status200OK, "The tutorials were found", typeof(IEnumerable<TutorialResource>))]
    public async Task<IActionResult> GetAllTutorials(CancellationToken cancellationToken)
    {
        var getAllTutorialsQuery = new GetAllTutorialsQuery();
        var tutorials = await tutorialQueryService.Handle(getAllTutorialsQuery, cancellationToken);
        var tutorialResources = tutorials.Select(TutorialResourceFromEntityAssembler.ToResourceFromEntity);
        return Ok(tutorialResources);
    }

    [HttpPost("{tutorialId:int}/videos")]
    [SwaggerOperation(
        Summary = "Add a video to a tutorial",
        Description = "Add a video to a tutorial",
        OperationId = "AddVideoToTutorial")]
    [SwaggerResponse(StatusCodes.Status201Created, "The video was added to the tutorial", typeof(TutorialResource))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "The video was not added to the tutorial")]
    public async Task<IActionResult> AddVideoToTutorial(
        [FromBody] AddVideoAssetToTutorialResource resource,
        [FromRoute] int tutorialId,
        CancellationToken cancellationToken)
    {
        var addVideoAssetToTutorialCommand =
            AddVideoAssetToTutorialCommandFromResourceAssembler.ToCommandFromResource(resource, tutorialId);
        var result = await tutorialCommandService.Handle(addVideoAssetToTutorialCommand, cancellationToken);

        return PublishingActionResultAssembler.ToActionResultFromAddVideoAssetToTutorialResult(
            this,
            result,
            _errorLocalizer,
            _problemDetailsFactory,
            updatedTutorial => CreatedAtAction(nameof(GetTutorialById), new { tutorialId = updatedTutorial.Id },
                TutorialResourceFromEntityAssembler.ToResourceFromEntity(updatedTutorial))
        );
    }
}