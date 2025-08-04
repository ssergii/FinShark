using System.Linq.Expressions;
using System.Reflection;

namespace FinShark.WebApi.Helpers;

public class FilterParam
{
    #region properties
    public string? FilterBy { get; set; }
    public string? Value { get; set; }
    #endregion

    #region methods
    private PropertyInfo? GetProperty<TModel>() where TModel : class, new()
    {
        if (string.IsNullOrEmpty(FilterBy))
            return null;

        var prop = typeof(TModel).GetProperties()
            .SingleOrDefault(x => x.Name.ToLower() == FilterBy.ToLower());
        if (prop == null)
            return null;

        return prop;
    }

    public bool Contains<TModel>(TModel model) where TModel : class, new()
    {
        if (string.IsNullOrEmpty(FilterBy))
            return true;

        var prop = GetProperty<TModel>();
        if (prop == null)
            throw new ArgumentException($"Property '{FilterBy}' not found in model '{typeof(TModel).Name}'");

        var filterVal = Value?.ToLower() ?? string.Empty;
        var modelVal = prop.GetValue(model)?.ToString().ToLower() ?? string.Empty;

        if (string.IsNullOrEmpty(filterVal) && string.IsNullOrEmpty(modelVal))
            return true;

        return modelVal.Contains(filterVal);
    }
    #endregion
}

public class OrderParam
{
    #region properties
    public string? OrderBy { get; set; }
    public bool IsDescending { get; set; }
    #endregion

    #region methods
    private PropertyInfo? GetProperty<TModel>() where TModel : class, new()
    {
        if (string.IsNullOrEmpty(OrderBy))
            return null;

        var prop = typeof(TModel).GetProperties()
            .SingleOrDefault(x => x.Name.ToLower() == OrderBy.ToLower());
        if (prop == null)
            return null;

        return prop;
    }

    public string Order<TModel>() where TModel : class, new()
    {
        if (string.IsNullOrEmpty(OrderBy))
        {
            var firstProp = typeof(TModel).GetProperties().First()?.Name;
            return firstProp; 
        }

        var prop = GetProperty<TModel>();
        if (prop == null)
            throw new ArgumentException($"Property '{OrderBy}' not found in model '{typeof(TModel).Name}'");

        return prop.Name;
    }
    #endregion
}

public class PageParam
{
    public int Number { get; set; } = 1;
    public int Size { get; set; } = 5;
}

public class QueryParameters
{
    #region properties
    public string? FilterBy { get; set; }
    public string? FilterVal { get; set; }

    public string? OrderBy { get; set; }
    public bool IsDescending { get; set; }

    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 5;
    #endregion

    #region methods
    public bool Filtering<TModel>(TModel model) where TModel : class, new()
    {
        if (string.IsNullOrEmpty(FilterBy))
            return true;

        var prop = model.GetType().GetProperties().Single(x => x.Name.ToLower() == FilterBy.ToLower());
        if (prop == null)
            return true;

        var filterVal = FilterVal?.ToString().ToLower() ?? string.Empty;
        var val = prop.GetValue(model)?.ToString().ToLower() ?? string.Empty;

        if (string.IsNullOrEmpty(val))
            return val == filterVal;

        var contains = val.ToLower().Contains(filterVal);

        return contains;
    }

    public string Ordering<TModel>() where TModel : class, new()
    {
        if (string.IsNullOrEmpty(OrderBy))
            return string.Empty;

        var prop = typeof(TModel).GetProperties().SingleOrDefault(x => x.Name.ToLower() == OrderBy.ToLower());
        if (prop == null)
            return string.Empty;

        return IsDescending ? $"{prop.Name} desc" : prop.Name;
    }
    #endregion
}

// {
//     var type = typeof(TModel);
//     if (!string.IsNullOrEmpty(SortBy))
//     {
//         var prop = type.GetProperties().Single(x => x.Name.ToLower() == SortBy.ToLower());
//         Func<TModel, object> expression = x => prop.GetValue(x);
//     }
// }