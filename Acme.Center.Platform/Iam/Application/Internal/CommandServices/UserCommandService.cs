using Acme.Center.Platform.Iam.Application.CommandServices;
using Acme.Center.Platform.Iam.Application.Internal.OutboundServices;
using Acme.Center.Platform.Iam.Domain.Model; // For IamError enum
using Acme.Center.Platform.Iam.Domain.Model.Aggregates;
using Acme.Center.Platform.Iam.Domain.Model.Commands;
using Acme.Center.Platform.Iam.Domain.Repositories;
using Acme.Center.Platform.Shared.Application.Model;
using Acme.Center.Platform.Shared.Domain.Repositories;
using Microsoft.Extensions.Localization; // For IStringLocalizer
using Acme.Center.Platform.Resources.Errors; // For ErrorMessages resource
using Microsoft.EntityFrameworkCore; // For DbUpdateException
using System.Threading;
using System.Threading.Tasks;
using System;

namespace Acme.Center.Platform.Iam.Application.Internal.CommandServices;

/**
 * <summary>
 *     The user command service
 * </summary>
 * <remarks>
 *     This class is used to handle user commands
 * </remarks>
 */
public class UserCommandService(
    IUserRepository userRepository,
    ITokenService tokenService,
    IHashingService hashingService,
    IUnitOfWork unitOfWork,
    IStringLocalizer<ErrorMessages> localizer) // Inject IStringLocalizer
    : IUserCommandService
{
    private readonly IStringLocalizer<ErrorMessages> _localizer = localizer;

    /**
     * <summary>
     *     Handle sign in command
     * </summary>
     * <param name="command">The sign in command</param>
     * <param name="cancellationToken">The cancellation token</param>
     * <returns>The authenticated user and the JWT token</returns>
     */
    public async Task<Result<(User user, string token)>> Handle(SignInCommand command, CancellationToken cancellationToken)
    {
        var user = await userRepository.FindByUsernameAsync(command.Username, cancellationToken);

        if (user == null || !hashingService.VerifyPassword(command.Password, user.PasswordHash))
            return Result<(User user, string token)>.Failure(IamError.InvalidCredentials, _localizer[nameof(IamError.InvalidCredentials)]);

        var token = tokenService.GenerateToken(user);

        return Result<(User user, string token)>.Success((user, token));
    }

    /**
     * <summary>
     *     Handle sign up command
     * </summary>
     * <param name="command">The sign up command</param>
     * <param name="cancellationToken">The cancellation token</param>
     * <returns>A confirmation message on successful creation.</returns>
     */
    public async Task<Result> Handle(SignUpCommand command, CancellationToken cancellationToken)
    {
        if (await userRepository.ExistsByUsername(command.Username, cancellationToken))
            return Result.Failure(IamError.UsernameAlreadyTaken, _localizer[nameof(IamError.UsernameAlreadyTaken), command.Username]);

        var hashedPassword = hashingService.HashPassword(command.Password);
        var user = new User(command.Username, hashedPassword);
        try
        {
            await userRepository.AddAsync(user, cancellationToken);
            await unitOfWork.CompleteAsync(cancellationToken);
            return Result.Success();
        }
        catch (OperationCanceledException)
        {
            return Result.Failure(IamError.OperationCancelled, _localizer[nameof(IamError.OperationCancelled)]);
        }
        catch (DbUpdateException)
        {
            // Log the exception details here if an ILogger is injected
            return Result.Failure(IamError.DatabaseError, _localizer[nameof(IamError.DatabaseError)]);
        }
        catch (Exception)
        {
            // Log the exception details here if an ILogger is injected
            return Result.Failure(IamError.InternalServerError, _localizer[nameof(IamError.InternalServerError)]);
        }
    }
}