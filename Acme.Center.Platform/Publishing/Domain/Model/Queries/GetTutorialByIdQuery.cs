namespace Acme.Center.Platform.Publishing.Domain.Model.Queries;

/// <summary>
///     Represents a query to get a tutorial by id in the ACME Learning Center Platform.
/// </summary>
/// <param name="TutorialId">
///     The id of the tutorial to get
/// </param>
public record GetTutorialByIdQuery(int TutorialId);