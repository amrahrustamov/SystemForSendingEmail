using Microsoft.AspNetCore.Mvc;
using Pustok.Database;
using Pustok.Database.Models;
using MimeKit;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using Pustok.Utilites;

namespace Pustok.Controllers.Client;

public class AccountController : Controller
{
    private readonly SmtpSettings _smtpSettings;
    private readonly PustokDbContext _pustokDbContext;

    public AccountController(IOptions<SmtpSettings> smtpSettings, PustokDbContext pustokDbContext)
    {
        _smtpSettings = smtpSettings.Value;
        _pustokDbContext = pustokDbContext;
    }

    public IActionResult Dashboard()
    {
        return View("~/Views/Client/Account/Dashboard.cshtml");
    }
    public IActionResult Orders()
    {
        return View("~/Views/Client/Account/Orders.cshtml");
    }

    public IActionResult Addresses()
    {
        return View("~/Views/Client/Account/Addresses.cshtml");
    }

    public IActionResult AccountDetails()
    {
        return View("~/Views/Client/Account/AccountDetails.cshtml");
    }

    public IActionResult Logout()
    {
        return RedirectToAction("index", "home");
    }

    [HttpGet("add")]
    public IActionResult Email()
    {
        var emails = _pustokDbContext.Emails.ToList();
        return View("~/Views/Client/Account/Email.cshtml", emails);
    }

    [HttpPost("add")]
    public IActionResult Email(Email model)
    {
        if (!ModelState.IsValid)
        {
            model = _pustokDbContext.Emails.FirstOrDefault();
            return View("~/Views/Client/Account/Email.cshtml", model);
        }

        Utilities utilities = new Utilities();
        string divideEmail = model.EmailAddress.ToString();
        string[] getEamil = utilities.GetEmails(divideEmail);

        var email = new Email(
            model.Title,
            model.Content,
            model.EmailAddress,
            model.SendingTime = DateTime.UtcNow
        );

        //var message = new MimeMessage();
        //message.From.Add(new MailboxAddress("Sender", "amrahrustamov94@yandex.com"));
        //message.To.Add(new MailboxAddress("Recipient", email.EmailAddress));
        //message.Subject = email.Title;

        //var builder = new BodyBuilder();
        //builder.TextBody = email.Content;
        //message.Body = builder.ToMessageBody();

        //using (var client = new SmtpClient())
        //{
        //    client.Connect(_smtpSettings.Server, _smtpSettings.Port, _smtpSettings.UseSsl);
        //    client.Authenticate(_smtpSettings.Username, _smtpSettings.Password);
        //    client.Send(message);
        //    client.Disconnect(true);
        //}

        _pustokDbContext.Emails.Add(email);
        _pustokDbContext.SaveChanges();

        var emails = _pustokDbContext.Emails.ToList();
        return View("~/Views/Client/Account/Email.cshtml", emails);
    }

    public IActionResult AllEmail()
    {
        var emails = _pustokDbContext.Emails.ToList();
        return View("~/Views/Client/Account/AllEmail.cshtml", emails);
    }
}