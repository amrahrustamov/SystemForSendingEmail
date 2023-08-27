using Microsoft.AspNetCore.Mvc;
using Pustok.Database;
using Pustok.ViewModels.Client;
using Pustok.Database.Models;
using Pustok.Utilites;
using Microsoft.EntityFrameworkCore;
using Pustok.ViewModels.Admin.Product;
using Pustok.Contracts;
using Pustok.Services.Abstracts;

namespace Pustok.Controllers.Client;

public class AccountController : Controller
{
    private readonly PustokDbContext _pustokDbContext;
    public Utilities _utilites;

    public AccountController(PustokDbContext pustokDbContext)
    {
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
        //logic

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

        var emails = new Email(
            model.Title,
            model.Content,
            model.EmailAddress,
            model.SendingTime = DateTime.UtcNow
        );

        _pustokDbContext.Emails.Add(emails);
        _pustokDbContext.SaveChanges();

        var email = _pustokDbContext.Emails.ToList();
        return View("~/Views/Client/Account/Email.cshtml", email);
    }



    //return View("~/Views/Client/Account/Email.cshtml", emails);
}

