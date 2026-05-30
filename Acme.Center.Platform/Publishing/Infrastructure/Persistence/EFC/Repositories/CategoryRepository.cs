using Acme.Center.Platform.Publishing.Domain.Model.Entities;
using Acme.Center.Platform.Publishing.Domain.Repositories;
using Acme.Center.Platform.Shared.Infrastructure.Persistence.EFC.Configuration;
using Acme.Center.Platform.Shared.Infrastructure.Persistence.EFC.Repositories;

namespace Acme.Center.Platform.Publishing.Infrastructure.Persistence.EFC.Repositories;

/// <summary>
///     Represents the category repository in the ACME Learning Center Platform.
/// </summary>
/// <param name="context">
///     The <see cref="AppDbContext" /> to use.
/// </param>
public class CategoryRepository(AppDbContext context) : BaseRepository<Category>(context), ICategoryRepository;