using Acme.Center.Platform.Publishing.Domain.Model.Commands;
using Acme.Center.Platform.Publishing.Domain.Model.Entities;

namespace Acme.Center.Platform.Publishing.Domain.Model.Aggregate;

/// <summary>
///     Represents the Tutorial aggregate root within the Publishing Bounded Context.
/// </summary>
/// <remarks>
///     This class encapsulates the core data and behavior of a tutorial,
///     acting as an aggregate root to ensure consistency of its contained entities.
/// </remarks>
public partial class Tutorial
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="Tutorial" /> aggregate.
    /// </summary>
    /// <param name="title">
    ///     The title of the tutorial.
    /// </param>
    /// <param name="summary">
    ///     A brief summary or description of the tutorial.
    /// </param>
    /// <param name="categoryId">
    ///     The unique identifier of the <see cref="Category" /> to which this tutorial belongs.
    /// </param>
    public Tutorial(string title, string summary, int categoryId) : this()
    {
        Title = title;
        Summary = summary;
        CategoryId = categoryId;
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="Tutorial" /> aggregate using a <see cref="CreateTutorialCommand" />.
    /// </summary>
    /// <param name="command">
    ///     The <see cref="CreateTutorialCommand" /> containing the data for creating the tutorial.
    /// </param>
    public Tutorial(CreateTutorialCommand command) : this(command.Title, command.Summary, command.CategoryId)
    {
    }

    /// <summary>
    ///     Gets the unique identifier of the tutorial.
    /// </summary>
    public int Id { get; }

    /// <summary>
    ///     Gets or sets the title of the tutorial.
    /// </summary>
    public string Title { get; private set; }

    /// <summary>
    ///     Gets or sets the summary or description of the tutorial.
    /// </summary>
    public string Summary { get; private set; }

    /// <summary>
    ///     Gets or sets the <see cref="Category" /> entity associated with this tutorial.
    ///     This is a navigation property typically loaded by the persistence mechanism.
    /// </summary>
    public Category Category { get; internal set; }

    /// <summary>
    ///     Gets or sets the foreign key for the associated <see cref="Category" />.
    /// </summary>
    public int CategoryId { get; private set; }
}