using ACME.LearningCenterPlatform.API.Profiles.Domain.Model.ValueObjects;

namespace ACME.LearningCenterPlatform.API.Profiles.Domain.Queries;

/// <summary>
/// Query to get profile by email 
/// </summary>
/// <param name="Email">
/// The email address of the profile to get. 
/// </param>
public record GetProfileByEmailQuery(EmailAddress Email);