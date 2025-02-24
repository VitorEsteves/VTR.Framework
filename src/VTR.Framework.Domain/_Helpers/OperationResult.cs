namespace VTR.Framework.Domain;

public class OperationResult(string message, MessageType severityMessage)
{
    public OperationResult(List<ValidationMessage> validations)
        : this("Validation error", MessageType.ValidationFailure)
    {
        Validations = validations;
    }

    public OperationResult(string message, string? messageCode, MessageType messageType)
        : this(message, messageType)
    {
        MessageCode = messageCode;
    }

    public string Message { get; protected set; } = message;

    public string? MessageCode { get; protected set; }

    public MessageType MessageType { get; protected set; } = severityMessage;

    public List<ValidationMessage>? Validations { get; protected set; }

    public List<InfoMessage>? Infos { get; protected set; }

    public void AddInfo(string message)
    {
        Infos ??= [];
        Infos.Add(new InfoMessage(message));
    }

    public static OperationResult CreateSuccess(string message, string? messageCode = null)
    {
        return new OperationResult(message, messageCode, MessageType.Success);
    }

    public static OperationResult CreateWarning(string message, string? messageCode = null)
    {
        return new OperationResult(message, messageCode, MessageType.Warning);
    }

    public static OperationResult CreateError(string message, string? messageCode = null)
    {
        return new OperationResult(message, messageCode, MessageType.Error);
    }

    public static OperationResult CreateValidationFailed(string propertyName, string failureMessage)
    {
        return new OperationResult([new(failureMessage, propertyName)]);
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