namespace VTR.Framework.Domain.Email;

public class EmailAttachment(Stream data, string contentId, string? contentType = null, string? filename = null)
{
    public Stream Data { get; internal set; } = data;
    public string ContentId { get; internal set; } = contentId;

    public string? Filename { get; internal set; } = filename;
    public string? ContentType { get; internal set; } = contentType;
}
