using Acme.Center.Platform.Iam.Domain.Model.Commands;
using Acme.Center.Platform.Iam.Domain.Model.Queries;
using Acme.Center.Platform.Iam.Domain.Services;

namespace Acme.Center.Platform.Iam.Interfaces.Acl.Services;

public class IamContextFacade(IUserCommandService userCommandService, IUserQueryService userQueryService) : IIamContextFacade
{
    public async Task<int> CreateUser(string username, string password)
    {
        var signUpCommand = new SignUpCommand(username, password);
        var signUpResult = await userCommandService.Handle(signUpCommand);
        if (signUpResult.IsFailure) return 0;
        var getUserByUsernameQuery = new GetUserByUsernameQuery(username);
        var result = await userQueryService.Handle(getUserByUsernameQuery);
        return result?.Id ?? 0;
    }

    public async Task<int> FetchUserIdByUsername(string username)
    {
        var getUserByUsernameQuery = new GetUserByUsernameQuery(username);
        var result = await userQueryService.Handle(getUserByUsernameQuery);
        return result?.Id ?? 0;
    }

    public async Task<string> FetchUsernameByUserId(int userId)
    {
        var getUserByIdQuery = new GetUserByIdQuery(userId);
        var result = await userQueryService.Handle(getUserByIdQuery);
        return result?.Username ?? string.Empty;
    }
}