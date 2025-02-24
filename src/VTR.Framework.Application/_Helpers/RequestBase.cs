using MediatR;

namespace VTR.Framework.Application.Contracts;

public class RequestBase<TResponse> : IRequest<TResponse>
{
    [Obsolete]
    public TResponse CreateResponseErrorUnavailableService()
    {
        return CreateResponse(new OperationResult("Service unavailable", MessageType.Error));
    }

    [Obsolete]
    public TResponse CreateResponseErrorRegisterNotFound(string name)
    {
        return CreateResponse(new OperationResult($"Record for '{name}' not found", MessageType.Error));
    }

    public TResponse CreateResponseError(string message)
    {
        return CreateResponse(OperationResult.CreateError(message));
    }

    public TResponse CreateResponseSuccess(string message)
    {
        return CreateResponse(OperationResult.CreateSuccess(message));
    }

    public TResponse CreateResponseWarning(string message)
    {
        return CreateResponse(OperationResult.CreateWarning(message));
    }

    public TResponse CreateResponseValidationFailed(string failureMessage, string? propertyName)
    {
        return CreateResponse(OperationResult.CreateValidationFailed(failureMessage, propertyName));
    }

    public TResponse CreateResponse(List<ValidationFailure> validationFailures)
    {
        var validations = validationFailures.ConvertAll(x => new ValidationMessage(x.ErrorMessage, x.PropertyName));

        var operationResult = new OperationResult(validations);

        return CreateResponse(operationResult);
    }

    public TResponse CreateResponse(OperationResult operationResult)
    {
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
#pragma warning disable CS8603 // Possible null reference return.
        return (TResponse)Activator.CreateInstance(typeof(TResponse), new[] { operationResult });
#pragma warning restore CS8603 // Possible null reference return.
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
    }
}