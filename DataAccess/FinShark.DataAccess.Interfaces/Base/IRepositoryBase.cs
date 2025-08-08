using System.Linq.Expressions;
using FinShark.DataAccess.Interfaces.QueryParams;

namespace FinShark.DataAccess.Interfaces.Base;

public interface IRepositoryBase<TModel> where TModel : class, new()
{
    Task<ICollection<TModel>> GetAsync(FilterParam? filter = null, OrderParam? order = null, PageParam? page = null, string? includeProperties = null);
    Task<TModel> GetSingleByAsync(Expression<Func<TModel, bool>> expression, string? includeProperties = null);
    Task<bool> ExistsAsync(Expression<Func<TModel, bool>> expression);
}