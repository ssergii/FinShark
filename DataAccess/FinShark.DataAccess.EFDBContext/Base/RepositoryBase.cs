using System.Linq.Expressions;
using FinShark.DataAccess.Interfaces.Base;
using FinShark.DataAccess.Interfaces.QueryParams;
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
    // public virtual async Task<ICollection<TModel>> GetAsync(string? includeProperties = null)
    // {
    //     if (string.IsNullOrEmpty(includeProperties))
    //         return await _dbSet.ToListAsync();

    //     IQueryable<TModel> query = _dbSet;
    //     var properties = includeProperties.Split(';', StringSplitOptions.RemoveEmptyEntries);
    //     foreach (var property in properties)
    //         query = query.Include(property);

    //     return await query.ToListAsync();
    // }

    public async Task<ICollection<TModel>> GetAsync(
        FilterParam? filter = null,
        OrderParam? order = null,
        PageParam? page = null,
        string? includeProperties = null)
    {
        IQueryable<TModel> query = _dbSet;

        if (filter != null)
        {
            var filterProp = GetPropName(filter.FilterBy);
            if (!string.IsNullOrEmpty(filter.FilterBy))
                query = _dbSet.Where(x => EF.Property<object>(x, filterProp).ToString().Contains(filter.Value));
        }

        if (order != null)
        {
            var orderProp = GetPropName(order.OrderBy);
            if (!string.IsNullOrEmpty(orderProp))
            {
                if (order.IsDescending)
                    query = query.OrderByDescending(x => EF.Property<object>(x, orderProp));
                else
                    query = query.OrderBy(x => EF.Property<object>(x, orderProp));
            }
        }

        if (page != null)
        {
            var skipNumber = (page.PageNumber - 1) * page.PageSize;
            query = query.Skip(skipNumber).Take(page.PageSize);
        }

        if (!string.IsNullOrEmpty(includeProperties))
        {
            var properties = includeProperties.Split(';', StringSplitOptions.RemoveEmptyEntries);
            foreach (var property in properties)
                query = query.Include(property);
        }

        return await query.ToListAsync();
    }

    public async Task<TModel> GetSingleByAsync(Expression<Func<TModel, bool>> expression, string? includeProperties = null)
    {
        if (string.IsNullOrEmpty(includeProperties))
            return await _dbSet.SingleAsync(expression);

        IQueryable<TModel> query = _dbSet;
        var properties = includeProperties.Split(';', StringSplitOptions.RemoveEmptyEntries);
        foreach (var property in properties)
            query = query.Include(property);

        return await query.SingleAsync(expression);
    }

    public async Task<bool> ExistsAsync(Expression<Func<TModel, bool>> expression)
    {
        IQueryable<TModel> query = _dbSet;

        var exist = await query.AnyAsync(expression);

        return exist;
    }

    protected async Task<int> SaveChangesAsync() => await _dbContext.SaveChangesAsync();
    
    private string? GetPropName(string propName)
    {
        if (string.IsNullOrEmpty(propName))
            return null;

        var prop = typeof(TModel).GetProperties()
            .SingleOrDefault(x => x.Name.ToLower() == propName.ToLower());
        if (prop == null)
            throw new ArgumentException($"Property '{propName}' not found in model '{typeof(TModel).Name}'");

        return prop.Name;
    }
    #endregion
}
