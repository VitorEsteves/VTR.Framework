namespace VTR.Framework.DataBase.EF;

public static class IQueryableExtension
{
    public static IOrderedQueryable<TSource> OrderByDynamic<TSource, TKey>(
        this IQueryable<TSource> source,
        Expression<Func<TSource, TKey>>[] keysSelector, bool ascending)
    {
        IOrderedQueryable<TSource> sort;

        if (ascending)
            sort = source.OrderBy(keysSelector[0]);
        else
            sort = source.OrderByDescending(keysSelector[0]);

        for (int i = 1; i < keysSelector.Length; i++)
        {
            sort = sort.ThenBy(keysSelector[i]);
        }

        return sort;
    }

    public static IOrderedQueryable<TSource> OrderBy<TSource>(
        this IQueryable<TSource> query, string? name, bool ascending,
        params Expression<Func<TSource, object?>>[] defaultSort)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return query.OrderByDynamic(defaultSort, ascending);
        }

        var propInfo = GetPropertyInfo(typeof(TSource), name);

        if (propInfo == null)
        {
            return query.OrderByDynamic(defaultSort, ascending);
        }

        var expr = GetOrderExpression(typeof(TSource), propInfo);

        string methodName = ascending ? "OrderBy" : "OrderByDescending";

        var method = typeof(Queryable).GetMethods().FirstOrDefault(m => m.Name == methodName && m.GetParameters().Length == 2);
        var genericMethod = method?.MakeGenericMethod(typeof(TSource), propInfo.PropertyType);

        var invoKeResult = genericMethod?.Invoke(null, new object[] { query, expr });

        if (invoKeResult is null)
            throw new NullReferenceException("GenericMethodInvoKeNull");

        return (IOrderedQueryable<TSource>)invoKeResult;

        PropertyInfo? GetPropertyInfo(Type objType, string nameProperty)
        {
            var properties = objType.GetProperties();
            var matchedProperty = properties.FirstOrDefault(p => p.Name.ToLower() == nameProperty.ToLower());

            return matchedProperty;
        }

        LambdaExpression GetOrderExpression(Type objType, PropertyInfo pi)
        {
            var paramExpr = Expression.Parameter(objType);
            var propAccess = Expression.PropertyOrField(paramExpr, pi.Name);
            var exprLambda = Expression.Lambda(propAccess, paramExpr);
            return exprLambda;
        }
    }

    public static async Task<(int totalRecords, List<TSource> records)> ToPagedListAsync<TSource>(
        this IQueryable<TSource> query,
        IFilterPaged paged,
        bool? defaultSortAsc = null,
        params Expression<Func<TSource, object?>>[] defaultSort)
    {
        if (paged.Sort is null && defaultSortAsc is not null)
            paged.SortAsc = defaultSortAsc.Value;

        var totalRecords = await query.CountAsync();

        if (paged.AllItems)
        {
            var all_records = await query.OrderBy(paged.Sort, paged.SortAsc, defaultSort[0]).ToListAsync();

            return (totalRecords, all_records);
        }

        if (paged.Page < 0)
        {
            paged.Page = 0;
        }

        if (paged.PageSize <= 0)
        {
            paged.PageSize = 10;
        }

        var finalQuery = query.OrderBy(paged.Sort, paged.SortAsc, defaultSort)
                              .Skip(paged.Page)
                              .Take(paged.PageSize);

        var records = await finalQuery.ToListAsync();

        return (totalRecords, records);
    }

    public static IQueryable<TEntity> Where<TEntity>(
        this IQueryable<TEntity> query, Expression<Func<TEntity, string?>> property, Filter? filter)
    {
        if (filter is null || !filter.HasValue)
            return query;

        var value = filter.Value?.ToString();
        if (value == "string")
            return query;

        string matchMode = filter.MatchMode?.ToLower() ?? "";

        string methodName = matchMode switch
        {
            "equals" or "notequals" => "Equals",
            "startswith" => "StartsWith",
            "endswith" => "EndsWith",
            _ => "Contains",
        };

        MethodInfo? mi = typeof(string).GetMethod(methodName, new Type[] { typeof(string) });

        if (mi is null)
            return query;

        MemberExpression m = ((MemberExpression)property.Body);
        ConstantExpression c = Expression.Constant(value, typeof(string));

        Expression call = matchMode.StartsWith("not") ? Expression.Not(Expression.Call(m, mi, c)) : Expression.Call(m, mi, c);
        return query.Where(Expression.Lambda<Func<TEntity, bool>>(call, property.Parameters));
    }

    public static IQueryable<TEntity> Where<TEntity>(
        this IQueryable<TEntity> query, Expression<Func<TEntity, bool?>> property, Filter? filter)
    {
        if (filter is null || !filter.HasValue)
            return query;

        var value = filter.Value?.ToString();

        if (value is null)
            return query;

        var boolValue = bool.Parse(value);

        var bd = Expression.Equal(property.Body, Expression.Constant(boolValue, typeof(bool?)));

        return query.Where(Expression.Lambda<Func<TEntity, bool>>(bd, property.Parameters));
    }

    public static IQueryable<TEntity> Where<TEntity>(
        this IQueryable<TEntity> query, Expression<Func<TEntity, int?>> property, Filter? filter)
    {
        if (filter is null || !filter.HasValue)
            return query;

        if (!int.TryParse(filter.Value?.ToString(), out int value))
            return query;
        string matchMode = filter.MatchMode?.ToLower() ?? "";

        Expression<Func<TEntity, bool>> queryFilter = matchMode switch
        {
            "notequals" => entity => property.Call()(entity) != value,
            "lt" => entity => property.Call()(entity) < value,
            "lte" => entity => property.Call()(entity) <= value,
            "gt" => entity => property.Call()(entity) > value,
            "gte" => entity => property.Call()(entity) >= value,
            _ => entity => property.Call()(entity) == value,
        };

        return query.Where(queryFilter.SubstituteMarker());
    }

    public static IQueryable<TEntity> Where<TEntity>(
        this IQueryable<TEntity> query, Expression<Func<TEntity, decimal?>> property, Filter? filter)
    {
        if (filter is null || !filter.HasValue)
            return query;

        if (!decimal.TryParse(filter.Value?.ToString(), out decimal value))
            return query;

        string matchMode = filter.MatchMode?.ToLower() ?? "";

        Expression<Func<TEntity, bool>> queryFilter = matchMode switch
        {
            "notequals" => entity => property.Call()(entity) != value,
            "lt" => entity => property.Call()(entity) < value,
            "lte" => entity => property.Call()(entity) <= value,
            "gt" => entity => property.Call()(entity) > value,
            "gte" => entity => property.Call()(entity) >= value,
            _ => entity => property.Call()(entity) == value,
        };

        return query.Where(queryFilter.SubstituteMarker());
    }

    public static IQueryable<TEntity> Where<TEntity>(
        this IQueryable<TEntity> query, Expression<Func<TEntity, DateTime?>> property, Filter? filter)
    {
        if (filter is null || !filter.HasValue)
            return query;

        if (!DateTime.TryParse(filter.Value?.ToString(), out DateTime value))
            return query;

        string matchMode = filter.MatchMode?.ToLower() ?? "";

        Expression<Func<TEntity, bool>> queryFilter = matchMode switch
        {
            _ => entity => property.Call()(entity) == value.Date,
        };

        return query.Where(queryFilter.SubstituteMarker());
    }
}