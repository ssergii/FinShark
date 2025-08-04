using System.Linq.Expressions;
using FinShark.DataAccess.Interfaces.Base;

namespace FinShark.DataAccess.EFDBContext.Base;

public class Repository<TModel> : RepositoryBase<TModel>, IRepository<TModel> where TModel : class, new()
{
    #region constructors
    public Repository(AppDBContext dbContext) : base(dbContext) { }
    #endregion

    #region methods
    public async Task CreateAsync(TModel item)
    {
        await _dbSet.AddAsync(item);
        await SaveChangesAsync();
    }

    public async Task<bool> DeleteAsync(Expression<Func<TModel, bool>> expression)
    {
        var items = _dbSet.Where(expression);
        _dbSet.RemoveRange(items);

        return await SaveChangesAsync() > 0;
    }

    public async Task UpdateAsync() => await SaveChangesAsync();
    #endregion
}
