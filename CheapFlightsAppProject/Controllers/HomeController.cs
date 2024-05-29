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


    public IActionResult Register()
    {

        return View();
    }
    public async Task<IActionResult> CheckLogin(string login, string haslo)
    {
        var md5 = System.Security.Cryptography.MD5.Create();
        byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(haslo);
        byte[] hashBytes = md5.ComputeHash(inputBytes);
        if (db.checkCredentials(login, Convert.ToHexString(hashBytes))) {
            return RedirectToAction("MainPage");
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


    public async Task<IActionResult> CheckRegister()
    {
        return null;
    }
    
    public IActionResult MainPage()
    {
        ViewBag.Name = "Joe";
        ViewBag.Surname = "Doe";
        return View();
       
    }

    public IActionResult MyAccount()
    {
        return View();
    }
    public IActionResult Logout()
    {
        return RedirectToAction("Index");
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}