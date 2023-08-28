using Microsoft.AspNetCore.Mvc;
using Pustok.Database;
using Pustok.Database.Models;
using MimeKit;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using Pustok.Utilites;
using Microsoft.AspNetCore.Components.Forms;

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
        try
        {
            var checkhing = utilities.CheckMessageInputs(model);

             var email = new Email(
                 checkhing.Title,
                 checkhing.Content,
                 checkhing.EmailAddress,
                 checkhing.SendingTime = DateTime.UtcNow
             );

          
             utilities.SendMessage(model,_smtpSettings);

             _pustokDbContext.Emails.Add(email);
             _pustokDbContext.SaveChanges();
        }
        catch
        {
            
        }
        finally
        {
           
        }
            var emails = _pustokDbContext.Emails.ToList();
        return View("~/Views/Client/Account/Email.cshtml", emails);
    }
    public IActionResult AllEmail()
    {
        var emails = _pustokDbContext.Emails.ToList();
        return View("~/Views/Client/Account/AllEmail.cshtml", emails);
    }
}