using System.Linq.Expressions;

namespace FinShark.DataAccess.Interfaces.Base;

public interface IRepositoryBase<TModel> where TModel : class, new()
{
    Task<ICollection<TModel>> GetAsync(string? includeProperties = null);
    Task<TModel> GetSingleByAsync(Expression<Func<TModel, bool>> expression, string? includeProperties = null);
    Task<ICollection<TModel>> GetByAsync(Expression<Func<TModel, bool>>? expression = null, Expression<Func<TModel, string>>? keySelector = null, int pageNumber = 1, int pageSize = 5, string? includeProperties = null);
    Task<bool> ExistsAsync(Expression<Func<TModel, bool>> expression);
}