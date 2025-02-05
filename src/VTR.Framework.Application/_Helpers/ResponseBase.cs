namespace VTR.Framework.Application.Contracts;

public class ResponseBase<T>(
    OperationResult operationResult) : ResponseBase(operationResult)
{
    public T? Record { get; set; }
}

public class ResponseBase(OperationResult operationResult)
{
    public OperationResult OperationResult { get; } = operationResult;
}