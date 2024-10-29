namespace ACME.LearningCenterPlatform.API.Profiles.Domain.Queries;

/// <summary>
/// Query to get profile by id 
/// </summary>
/// <param name="ProfileId">
/// The id of the profile to get.
/// </param>
public record GetProfileByIdQuery(int ProfileId);