using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using CheapFlightsAppProject.Models;

namespace CheapFlightsAppProject.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult LoginForm()
    {
        return View();
    }

    public IActionResult WorldMap()
    {
        return View();
    }
    public async Task<IActionResult> CheckLogin(string login, string haslo)
    {
       // foreach (Login l in logins)
       //{
       //     string hashedPassword = Database.Utils.CalculateMD5Hash(haslo);
       //     Console.WriteLine($"HASHED PASSWORD : {hashedPassword}");
       //     if (login == l.Username && hashedPassword == l.Password)
       //     {
                // var claims = new List<Claim>
                // {
                //     new Claim(ClaimTypes.Name, login)
                // };

                // var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                //
                // var principal = new ClaimsPrincipal(identity);
                //
                // await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
                //
                // HttpContext.Session.SetString("login", (new DateTimeOffset(DateTime.Now.AddDays(7))).ToString());
                return RedirectToAction("LoggedIn");
            //}
        //}
       // return RedirectToAction("Index");
            
    }
    public IActionResult LoggedIn()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    public IActionResult Login()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}