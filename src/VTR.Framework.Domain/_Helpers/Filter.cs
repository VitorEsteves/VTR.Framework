namespace VTR.Framework.Domain;

public class Filter
{
    public object? Value { get; set; }

    public bool HasValue => !string.IsNullOrWhiteSpace(Value?.ToString());

    public string? MatchMode { get; set; }
}