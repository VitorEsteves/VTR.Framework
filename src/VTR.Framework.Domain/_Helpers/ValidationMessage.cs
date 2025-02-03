namespace VTR.Framework.Domain;

public class ValidationMessage(string failureMessage, string? propertyName)
{
    public string FailureMessage { get; set; } = failureMessage;
    public string? PropertyName { get; set; } = propertyName;
}