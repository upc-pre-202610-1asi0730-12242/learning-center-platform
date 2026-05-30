using Acme.Center.Platform.IAM.Domain.Model.Aggregates;
using Acme.Center.Platform.IAM.Interfaces.REST.Resources;

namespace Acme.Center.Platform.IAM.Interfaces.REST.Transform;

public static class UserResourceFromEntityAssembler
{
    public static UserResource ToResourceFromEntity(User user)
    {
        return new UserResource(user.Id, user.Username);
    }
}