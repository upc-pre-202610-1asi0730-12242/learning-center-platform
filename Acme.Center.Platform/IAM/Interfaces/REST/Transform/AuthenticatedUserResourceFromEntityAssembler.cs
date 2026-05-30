using Acme.Center.Platform.IAM.Domain.Model.Aggregates;
using Acme.Center.Platform.IAM.Interfaces.REST.Resources;

namespace Acme.Center.Platform.IAM.Interfaces.REST.Transform;

public static class AuthenticatedUserResourceFromEntityAssembler
{
    public static AuthenticatedUserResource ToResourceFromEntity(
        User user, string token)
    {
        return new AuthenticatedUserResource(user.Id, user.Username, token);
    }
}