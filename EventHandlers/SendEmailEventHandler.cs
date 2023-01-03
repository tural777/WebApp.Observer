using MediatR;
using System.Net;
using System.Net.Mail;
using WebApp.Observer.Events;


namespace WebApp.Observer.EventHandlers;


public class SendEmailEventHandler : INotificationHandler<UserCreatedEvent>
{
    private readonly ILogger<SendEmailEventHandler> _logger;

    public SendEmailEventHandler(ILogger<SendEmailEventHandler> logger) => _logger = logger;


    public Task Handle(UserCreatedEvent notification, CancellationToken cancellationToken)
    {
        var mailMessage = new MailMessage();

        var smptClient = new SmtpClient("srvm11.trwww.com");

        mailMessage.From = new MailAddress("example@kariyersistem.com");

        mailMessage.To.Add(new MailAddress(notification.AppUser.Email));

        mailMessage.Subject = "Saytimiza xoş geldiniz.";

        mailMessage.Body = "<p>Saytimizin ümumi qaydaları : bla bla....</p>";

        mailMessage.IsBodyHtml = true;
        smptClient.Port = 587;
        smptClient.Credentials = new NetworkCredential("example@kariyersistem.com", "Password12*");

        smptClient.Send(mailMessage);
        _logger.LogInformation($"Email was send to user :{notification.AppUser.UserName}");

        return Task.CompletedTask;
    }
}