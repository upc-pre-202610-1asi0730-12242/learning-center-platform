using Acme.Center.Platform.IAM.Domain.Model.Commands;
using Acme.Center.Platform.IAM.Interfaces.REST.Resources;

namespace Acme.Center.Platform.IAM.Interfaces.REST.Transform;

public static class SignInCommandFromResourceAssembler
{
    public static SignInCommand ToCommandFromResource(SignInResource resource)
    {
        return new SignInCommand(resource.Username, resource.Password);
    }
}