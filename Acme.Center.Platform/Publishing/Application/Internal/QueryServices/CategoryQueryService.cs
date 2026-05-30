using Acme.Center.Platform.Publishing.Domain.Model.Entities;
using Acme.Center.Platform.Publishing.Domain.Model.Queries;
using Acme.Center.Platform.Publishing.Domain.Repositories;
using Acme.Center.Platform.Publishing.Domain.Services;

namespace Acme.Center.Platform.Publishing.Application.Internal.QueryServices;

/// <summary>
///     Represents the category query service in the ACME Learning Center Platform.
/// </summary>
/// <param name="categoryRepository">
///     The <see cref="ICategoryRepository" /> to use.
/// </param>
public class CategoryQueryService(ICategoryRepository categoryRepository)
    : ICategoryQueryService
{
    /// <inheritdoc />
    public async Task<Category?> Handle(GetCategoryByIdQuery query)
    {
        return await categoryRepository.FindByIdAsync(query.CategoryId);
    }

    /// <inheritdoc />
    public async Task<IEnumerable<Category>> Handle(GetAllCategoriesQuery query)
    {
        return await categoryRepository.ListAsync();
    }
}