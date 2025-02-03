namespace VTR.Framework.Application.Contracts;

public class ResponseBase(OperationResult operationResult)
{
    public OperationResult OperationResult { get; } = operationResult;
}