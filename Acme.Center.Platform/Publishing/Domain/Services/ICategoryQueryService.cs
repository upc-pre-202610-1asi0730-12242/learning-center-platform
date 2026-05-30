using Acme.Center.Platform.Publishing.Domain.Model.Entities;
using Acme.Center.Platform.Publishing.Domain.Model.Queries;

namespace Acme.Center.Platform.Publishing.Domain.Services;

/// <summary>
///     Represents the category query service in the ACME Learning Center Platform.
/// </summary>
public interface ICategoryQueryService
{
    /// <summary>
    ///     Handles the get category by id query in the ACME Learning Center Platform.
    /// </summary>
    /// <param name="query">
    ///     The <see cref="GetCategoryByIdQuery" /> query to handle.
    /// </param>
    /// <returns></returns>
    Task<Category?> Handle(GetCategoryByIdQuery query);

    /// <summary>
    ///     Handles the get all categories query in the ACME Learning Center Platform.
    /// </summary>
    /// <param name="query">
    ///     The <see cref="GetAllCategoriesQuery" /> query to handle.
    /// </param>
    /// <returns>
    ///     A collection of all categories in the platform.
    /// </returns>
    Task<IEnumerable<Category>> Handle(GetAllCategoriesQuery query);
}