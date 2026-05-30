using Acme.Center.Platform.Profiles.Domain.Model.Aggregates;
using Acme.Center.Platform.Profiles.Domain.Model.Commands;

namespace Acme.Center.Platform.Profiles.Application.CommandServices;

/// <summary>
/// Profile command service interface 
/// </summary>
public interface IProfileCommandService
{
    /// <summary>
    /// Handle create profile command 
    /// </summary>
    /// <param name="command">
    /// The <see cref="CreateProfileCommand"/> command
    /// </param>
    /// <returns>
    /// The <see cref="Profile"/> object with the created profile
    /// </returns>
    Task<Profile?> Handle(CreateProfileCommand command);
}