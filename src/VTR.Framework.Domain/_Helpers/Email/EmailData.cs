namespace VTR.Framework.Domain.Email;

public class EmailData(EMailAddress fromAddress, string subject, string body)
{
    public List<EMailAddress> ToAddresses { get; set; } = [];
    public List<EMailAddress> CcAddresses { get; set; } = [];
    public List<EMailAddress> BccAddresses { get; set; } = [];
    public List<EMailAddress> ReplyToAddresses { get; set; } = [];
    public List<EmailAttachment> Attachments { get; set; } = [];
    public EMailAddress FromAddress { get; set; } = fromAddress;
    public string Subject { get; set; } = subject;
    public string Body { get; set; } = body;
    public string? PlaintextAlternativeBody { get; set; }
    public EmailPriority Priority { get; set; }
    public List<string> Tags { get; set; } = [];

    public bool IsHtml { get; set; }
    public IDictionary<string, string> Headers { get; set; } = new Dictionary<string, string>();
}