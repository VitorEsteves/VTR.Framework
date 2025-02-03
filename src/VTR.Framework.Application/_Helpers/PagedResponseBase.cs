namespace VTR.Framework.Application.Contracts;

public class PagedResponseBase<T> : ResponseBase
{
    public PagedResponseBase(OperationResult operationResult)
        : base(operationResult) { }

    public List<T>? Records { get; set; }

    public int TotalRecords { get; set; }
}