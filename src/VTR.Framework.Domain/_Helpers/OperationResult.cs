namespace VTR.Framework.Domain;

public class OperationResult(string message, MessageType severityMessage)
{
    public OperationResult(string message, Exception exception)
        : this(message, MessageType.Error)
    {
        Exception = exception;
    }

    public OperationResult(List<ValidationMessage> validations)
        : this("Validation error", MessageType.ValidationFailure)
    {
        Validations = validations;
    }

    public OperationResult(string message, string messageCode)
        : this(message, MessageType.Warning)
    {
        MessageCode = messageCode;
    }

    public string Message { get; protected set; } = message;

    public string? MessageCode { get; protected set; }

    public MessageType MessageType { get; protected set; } = severityMessage;

    public Exception? Exception { get; protected set; }

    public List<ValidationMessage>? Validations { get; protected set; }

    public static OperationResult Success(string message)
    {
        return new OperationResult(message, MessageType.Success);
    }

    public static OperationResult Create(object instance, string successMessage)
    {
        var validationsResult = new List<ValidationResult>();
        var contexto = new ValidationContext(instance, null, null);
        Validator.TryValidateObject(instance, contexto, validationsResult, true);

        var validations = validationsResult.ConvertAll(x =>
            new ValidationMessage(x.ErrorMessage ?? string.Empty, x.MemberNames?.FirstOrDefault()));

        if (validations.Count > 0)
        {
            return new OperationResult(validations);
        }
        else
        {
            return new OperationResult(successMessage, MessageType.Success);
        }
    }
}