using System.Diagnostics;
using CheapFlightsAppProject.Database;
using Microsoft.AspNetCore.Mvc;
using CheapFlightsAppProject.Models;

namespace CheapFlightsAppProject.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private DbHandler db;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
        db = new DbHandler();
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult LoginForm()
    {
        return View();
    }
    public IActionResult SignUpForm()
    {
        return View();
    }

    public IActionResult FlightDetails(string arg) {
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
        var md5 = System.Security.Cryptography.MD5.Create();
        byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(haslo);
        byte[] hashBytes = md5.ComputeHash(inputBytes);
        if (db.checkCredentials(login, Convert.ToHexString(hashBytes))) {
            return RedirectToAction("LoggedIn");
        }
        else {
            return RedirectToAction("Index");
        }
    }

    public async Task<IActionResult> SignUp(string login, string haslo, string hasloConf) {
        if (haslo == hasloConf) {
            // Czy trzeba uzywac using tutaj??
            var md5 = System.Security.Cryptography.MD5.Create();
            byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(haslo);
            byte[] hashBytes = md5.ComputeHash(inputBytes);
            
            if (db.createUser(login, Convert.ToHexString(hashBytes))) {
                return RedirectToAction("LoginForm");
            }
        }
        return RedirectToAction("Index");
    }

    public async Task<IActionResult> SearchFlight(string departureCity, string destinationCity, string flightDate) {
        ViewBag.flights = db.searchFlights(departureCity, destinationCity, flightDate);
        return View("FlightDetails");
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