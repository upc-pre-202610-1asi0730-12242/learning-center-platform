using Acme.Center.Platform.Iam.Application.Internal.OutboundServices;
using Acme.Center.Platform.Iam.Domain.Model.Aggregates;
using Acme.Center.Platform.Iam.Domain.Model.Commands;
using Acme.Center.Platform.Iam.Domain.Repositories;
using Acme.Center.Platform.Iam.Domain.Services;
using Acme.Center.Platform.Shared.Application.Model;
using Acme.Center.Platform.Shared.Domain.Repositories;

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
    IUnitOfWork unitOfWork)
    : IUserCommandService
{
    /**
     * <summary>
     *     Handle sign in command
     * </summary>
     * <param name="command">The sign in command</param>
     * <returns>The authenticated user and the JWT token</returns>
     */
    public async Task<Result<(User user, string token)>> Handle(SignInCommand command)
    {
        var user = await userRepository.FindByUsernameAsync(command.Username);

        if (user == null || !hashingService.VerifyPassword(command.Password, user.PasswordHash))
            return Result<(User user, string token)>.Failure("Invalid username or password");

        var token = tokenService.GenerateToken(user);

        return Result<(User user, string token)>.Success((user, token));
    }

    /**
     * <summary>
     *     Handle sign up command
     * </summary>
     * <param name="command">The sign up command</param>
     * <returns>A confirmation message on successful creation.</returns>
     */
    public async Task<Result> Handle(SignUpCommand command)
    {
        if (userRepository.ExistsByUsername(command.Username))
            return Result.Failure($"Username {command.Username} is already taken");

        var hashedPassword = hashingService.HashPassword(command.Password);
        var user = new User(command.Username, hashedPassword);
        try
        {
            await userRepository.AddAsync(user);
            await unitOfWork.CompleteAsync();
            return Result.Success();
        }
        catch (Exception e)
        {
            return Result.Failure($"An error occurred while creating user: {e.Message}");
        }
    }
}