namespace VTR.Framework.Domain.Contracts;

public interface IFilterPaged
{
    int Page { get; set; }

    int PageSize { get; set; }

    string? Sort { get; set; }

    bool SortAsc { get; set; }

    bool AllItems { get; set; }

    string? GlobalValue { get; set; }
}