using Acme.Center.Platform.Publishing.Domain.Model.Commands;
using Acme.Center.Platform.Publishing.Interfaces.REST.Resources;

namespace Acme.Center.Platform.Publishing.Interfaces.REST.Transform;

public static class CreateCategoryCommandFromResourceAssembler
{
    public static CreateCategoryCommand ToCommandFromResource(CreateCategoryResource resource)
    {
        return new CreateCategoryCommand(resource.Name);
    }
}