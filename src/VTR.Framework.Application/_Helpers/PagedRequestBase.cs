namespace VTR.Framework.Application.Contracts;

public class PagedRequestBase<TResponse> : RequestBase<TResponse>, IFilterPaged
{
    [FromQuery]
    public int Page { get; set; } = 0;

    [FromQuery]
    public int PageSize { get; set; } = 10;

    [FromQuery]
    public string? Sort { get; set; }

    [FromQuery]
    public bool SortAsc { get; set; }

    [FromQuery]
    public bool AllItems { get; set; }

    [FromQuery]
    public string? GlobalValue { get; set; }
}