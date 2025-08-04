using System.Linq.Expressions;

namespace FinShark.DataAccess.Interfaces.Base;

public interface IRepository<TModel> : IRepositoryBase<TModel> where TModel : class, new()
{
    Task CreateAsync(TModel model);
    Task UpdateAsync();
    Task<bool> DeleteAsync(Expression<Func<TModel, bool>> expression);
}