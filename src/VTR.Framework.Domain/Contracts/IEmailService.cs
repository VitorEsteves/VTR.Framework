namespace VTR.Framework.Domain.Contracts;

public interface IEmailService
{
    Task SendAsync(EmailData email, CancellationToken token);
}