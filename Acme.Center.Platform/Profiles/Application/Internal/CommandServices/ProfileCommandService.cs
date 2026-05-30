using Acme.Center.Platform.Profiles.Application.CommandServices;
using Acme.Center.Platform.Profiles.Domain.Model.Aggregates;
using Acme.Center.Platform.Profiles.Domain.Model.Commands;
using Acme.Center.Platform.Profiles.Domain.Repositories;
using Acme.Center.Platform.Shared.Domain.Repositories;

namespace Acme.Center.Platform.Profiles.Application.Internal.CommandServices;

/// <summary>
/// Profile command service 
/// </summary>
/// <param name="profileRepository">
/// Profile repository
/// </param>
/// <param name="unitOfWork">
/// Unit of work
/// </param>
public class ProfileCommandService(
    IProfileRepository profileRepository, 
    IUnitOfWork unitOfWork) 
    : IProfileCommandService
{
    /// <inheritdoc />
    public async Task<Profile?> Handle(CreateProfileCommand command)
    {
        var profile = new Profile(command);
        try
        {
            await profileRepository.AddAsync(profile);
            await unitOfWork.CompleteAsync();
            return profile;
        } catch (Exception e)
        {
            // Log error
            return null;
        }
    }
}