namespace VTR.Framework.Domain.Email;

public class EmailData(EmailAddress fromAddress, string subject, string body)
{
    public List<EmailAddress> ToAddresses { get; set; } = [];
    public List<EmailAddress> CcAddresses { get; set; } = [];
    public List<EmailAddress> BccAddresses { get; set; } = [];
    public List<EmailAddress> ReplyToAddresses { get; set; } = [];
    public List<EmailAttachment> Attachments { get; set; } = [];
    public EmailAddress FromAddress { get; set; } = fromAddress;
    public string Subject { get; set; } = subject;
    public string Body { get; set; } = body;
    public string? PlaintextAlternativeBody { get; set; }
    public EmailPriority Priority { get; set; }
    public List<string> Tags { get; set; } = [];

    public bool IsHtml { get; set; }
    public IDictionary<string, string> Headers { get; set; } = new Dictionary<string, string>();
}