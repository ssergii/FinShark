using System.Linq.Expressions;
using FinShark.DataAccess.Interfaces.Base;
using Microsoft.EntityFrameworkCore;

namespace FinShark.DataAccess.EFDBContext.Base;

public class RepositoryBase<TModel> : IRepositoryBase<TModel> where TModel : class, new()
{
    #region fields
    private readonly AppDBContext _dbContext;
    protected readonly DbSet<TModel> _dbSet;
    #endregion

    #region constructors
    public RepositoryBase(AppDBContext dbContext)
    {
        _dbContext = dbContext;
        _dbSet = _dbContext.Set<TModel>();

    }
    #endregion

    #region methods
    public virtual async Task<ICollection<TModel>> GetAsync(string? includeProperties = null)
    {
        if (string.IsNullOrEmpty(includeProperties))
            return await _dbSet.ToListAsync();

        IQueryable<TModel> query = _dbSet;
        var properties = includeProperties.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
        foreach (var property in properties)
            query = query.Include(property);

        return await query.ToListAsync();
    }

    public async Task<TModel> GetSingleByAsync(Expression<Func<TModel, bool>> expression, string? includeProperties = null)
    {
        if (string.IsNullOrEmpty(includeProperties))
            return await _dbSet.SingleAsync(expression);

        IQueryable<TModel> query = _dbSet;
        var properties = includeProperties.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
        foreach (var property in properties)
            query = query.Include(property);

        return await query.SingleAsync(expression);
    }

    public async Task<ICollection<TModel>> GetByAsync(
        Expression<Func<TModel, bool>>? expression = null,
        Expression<Func<TModel, string>>? keySelector = null,
        int pageNumber = 1,
        int pageSize = 5,
        string? includeProperties = null)
    {
        if (string.IsNullOrEmpty(includeProperties))
            return await _dbSet.ToListAsync();

        IQueryable<TModel> query = _dbSet;
        var properties = includeProperties.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
        foreach (var property in properties)
            query = query.Include(property);

        if (expression != null)
            query = query.Where(expression);
        // if (keySelector != null)
        //     query = query.OrderBy(keySelector);

        // // ordering _dbSet
        // var stocks = _dbSet.Where(x => EF.Property<string>(x, "PropName").Contains("AAPL"));
        // stocks = stocks.OrderBy(x => EF.Property<object>(x, "PropName"));

        // Apply pagination
        // var skipNumber = (pageNumber - 1) * pageSize;
        // query = query.Skip(skipNumber).Take(pageSize);

        return await query.ToListAsync();
    }

    public async Task<bool> ExistsAsync(Expression<Func<TModel, bool>> expression)
    {
        IQueryable<TModel> query = _dbSet;

        var exist = await query.AnyAsync(expression);

        return exist;
    }

    protected async Task<int> SaveChangesAsync() => await _dbContext.SaveChangesAsync();
    #endregion
}
