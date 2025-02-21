using System.Net.Mail;
using System.Net.Mime;
using System.Text;

namespace VTR.Framework.Application.Services;

public class EmailService(SmtpClient smtpClient) : IEmailService
{
    public Task SendAsync(
        EmailData email,
        CancellationToken token = default)
    {        
        MailMessage message = CreateMailMessage(email);

        return Task.Run(() => SendImplAsync(message, token), token);
    }

    private async Task SendImplAsync(
        MailMessage message,
        CancellationToken token)
    {
        token.ThrowIfCancellationRequested();

        var tcs = new TaskCompletionSource<bool>();
        SendCompletedEventHandler? handler = null;
        void unsubscribe() => smtpClient.SendCompleted -= handler;

        handler = async (s, e) =>
        {
            unsubscribe();

            // a hack to complete the handler asynchronously
            await Task.Yield();

            if (e.UserState != tcs)
                tcs.TrySetException(new InvalidOperationException("Unexpected UserState"));
            else if (e.Cancelled)
                tcs.TrySetCanceled();
            else if (e.Error != null)
                tcs.TrySetException(e.Error);
            else
                tcs.TrySetResult(true);
        };

        smtpClient.SendCompleted += handler;
        try
        {
            smtpClient.SendAsync(message, tcs);
            using (token.Register(() => smtpClient.SendAsyncCancel(), useSynchronizationContext: false))
            {
                await tcs.Task;
            }
        }
        finally
        {
            unsubscribe();
        }
    }

    private MailMessage CreateMailMessage(EmailData email)
    {
        MailMessage? message = null;

        // Smtp seems to require the HTML version as the alternative.
        if (!string.IsNullOrEmpty(email.PlaintextAlternativeBody))
        {
            message = new MailMessage
            {
                Subject = email.Subject,
                Body = email.PlaintextAlternativeBody,
                IsBodyHtml = false,
                From = new MailAddress(email.FromAddress.EmailAddress, email.FromAddress.Name)
            };

            var mimeType = new ContentType("text/html; charset=UTF-8");
            AlternateView alternate = AlternateView.CreateAlternateViewFromString(email.Body ?? string.Empty, mimeType);
            message.AlternateViews.Add(alternate);
        }
        else
        {
            message = new MailMessage
            {
                Subject = email.Subject,
                Body = email.Body,
                IsBodyHtml = email.IsHtml,
                BodyEncoding = Encoding.UTF8,
                SubjectEncoding = Encoding.UTF8,
                From = new MailAddress(email.FromAddress.EmailAddress, email.FromAddress.Name)
            };
        }

        foreach (var header in email.Headers)
        {
            message.Headers.Add(header.Key, header.Value);
        }

        email.ToAddresses.ForEach(x =>
        {
            message.To.Add(new MailAddress(x.EmailAddress, x.Name));
        });

        email.CcAddresses.ForEach(x =>
        {
            message.CC.Add(new MailAddress(x.EmailAddress, x.Name));
        });

        email.BccAddresses.ForEach(x =>
        {
            message.Bcc.Add(new MailAddress(x.EmailAddress, x.Name));
        });

        email.ReplyToAddresses.ForEach(x =>
        {
            message.ReplyToList.Add(new MailAddress(x.EmailAddress, x.Name));
        });

        switch (email.Priority)
        {
            case EmailPriority.Low:
                message.Priority = MailPriority.Low;
                break;
            case EmailPriority.Normal:
                message.Priority = MailPriority.Normal;
                break;
            case EmailPriority.High:
                message.Priority = MailPriority.High;
                break;
        }

        email.Attachments.ForEach(x =>
        {
            Attachment a = new(x.Data, x.Filename, x.ContentType)
            {
                ContentId = x.ContentId
            };

            message.Attachments.Add(a);
        });

        return message;
    }
}