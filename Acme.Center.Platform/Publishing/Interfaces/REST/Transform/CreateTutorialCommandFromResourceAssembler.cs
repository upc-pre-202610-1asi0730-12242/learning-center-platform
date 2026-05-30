using Acme.Center.Platform.Publishing.Domain.Model.Commands;
using Acme.Center.Platform.Publishing.Interfaces.REST.Resources;

namespace Acme.Center.Platform.Publishing.Interfaces.REST.Transform;

public static class CreateTutorialCommandFromResourceAssembler
{
    public static CreateTutorialCommand ToCommandFromResource(CreateTutorialResource resource)
    {
        return new CreateTutorialCommand(resource.Title, resource.Summary, resource.CategoryId);
    }
}