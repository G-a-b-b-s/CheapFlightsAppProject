using System.Collections.Immutable;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using CheapFlightsAppProject.Database;
using Microsoft.AspNetCore.Mvc;
using CheapFlightsAppProject.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;

namespace CheapFlightsAppProject.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private DbHandler _db;
    private readonly IConfiguration _config;
    
    public HomeController(ILogger<HomeController> logger, IConfiguration config)
    {
        _logger = logger;
        _db = new DbHandler();
        _config = config;
    }

    [AllowAnonymous]
    public IActionResult Index()
    {
        return View();
    }

    [AllowAnonymous]
    public IActionResult LoginForm()
    {
        return View();
    }
    
    [Authorize]
    public IActionResult FlightDetails(string arg)
    {
        ViewBag.role = HttpContext.User.Claims.FirstOrDefault(x=>x.Type==ClaimTypes.Role)?.Value;
        return View();
    }

    [AllowAnonymous]
    public IActionResult SignUpForm()
    {
        return View();
    }
    
    [AllowAnonymous]
    public async Task<IActionResult> CheckLogin(string login, string haslo)
    {
        var md5 = System.Security.Cryptography.MD5.Create();
        byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(haslo);
        byte[] hashBytes = md5.ComputeHash(inputBytes);
        User u = _db.GetUser(login, Convert.ToHexString(hashBytes));
        if (u!=null) {
            var token = Generate(u);
            HttpContext.Session.SetString("JWToken", token);
            return RedirectToAction("MyAccount");
        }
        else {
            return RedirectToAction("Index");
        }
    }

    [AllowAnonymous]
    public async Task<IActionResult> SignUp( string login, string haslo, string hasloConf) {
        if (haslo == hasloConf) {
            var md5 = System.Security.Cryptography.MD5.Create();
            byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(haslo);
            byte[] hashBytes = md5.ComputeHash(inputBytes);
            if (_db.CreateUser(login, Convert.ToHexString(hashBytes))) {
                TempData["Message"] = "User created successfully!";
                return RedirectToAction("LoginForm");
            }
        }
        return RedirectToAction("Index");
    }

    [Authorize]
    public async Task<IActionResult> SearchFlight(string departureCity, string destinationCity, string flightDate) {
        ViewBag.flights = _db.SearchFlights(departureCity, destinationCity, flightDate);
        ViewBag.role = HttpContext.User.Claims.FirstOrDefault(x=>x.Type==ClaimTypes.Role)?.Value;
        return View("FlightDetails");
    }
    
    [Authorize]
    public async Task<IActionResult> FlightsBrowser() {
        ViewBag.role = HttpContext.User.Claims.FirstOrDefault(x=>x.Type==ClaimTypes.Role)?.Value;
        return View("SearchFlight");
    }


    public async Task<IActionResult> CheckRegister()
    {
        return null;
    }
    
    [Authorize]
    public IActionResult MainPage()
    {
        return View();
    }
    
    [Authorize]
    public IActionResult AdminMainPage()
    {
        return View();
       
    }

    [Authorize]
    public IActionResult MyAccount()
    {
        ViewBag.role = HttpContext.User.Claims.FirstOrDefault(x=>x.Type==ClaimTypes.Role)?.Value;
        string username = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
        ViewBag.username = username;
        ViewBag.flights = _db.SearchSavedFlights(username);
        return View();
    }
    
    public IActionResult Logout()
    {
        return RedirectToAction("Index");
    }

    [Authorize]
    public IActionResult ManageFlights()
    {
        ViewBag.role = HttpContext.User.Claims.FirstOrDefault(x=>x.Type==ClaimTypes.Role)?.Value;
        return View();
    }

    [Authorize]
    public IActionResult AddFlight()
    {
        ViewBag.role = HttpContext.User.Claims.FirstOrDefault(x=>x.Type==ClaimTypes.Role)?.Value;
        return View();
    }
    
    [Authorize]
    public IActionResult EditFlight()
    {
        ViewBag.role = HttpContext.User.Claims.FirstOrDefault(x=>x.Type==ClaimTypes.Role)?.Value;
        return View();
    }
    
    [Authorize]
    public IActionResult DeleteFlight()
    {
        ViewBag.role = HttpContext.User.Claims.FirstOrDefault(x=>x.Type==ClaimTypes.Role)?.Value;
        return View();
    }
    
    [Authorize]
    public IActionResult ModifyDataBase(Flight flight)
    {
        var referrer = Request.Headers["Referer"].ToString();
        if (referrer.Contains("AddFlight"))
        {
            if (_db.AddFlight(flight))
            {
                TempData["Message"] = "Flight added successfully!";
            }
        }
        else if (referrer.Contains("DeleteFlight"))
        {
            if (_db.DeleteFlight(flight))
            {
                TempData["Message"] = "Flight deleted successfully!";
            }
        }
        else 
        {
            if (_db.EditFlight(flight))
            {
                TempData["Message"] = "Flight edited successfully!";
            }
            
        }

        if (TempData["Message"] == null)
        {
            TempData["Message"] = "An error occurred!";
        }
        return RedirectToAction("AdminMainPage");   
    }

    [Authorize]
    public IActionResult SaveFlight(string myData) {
        _db.SaveFlight(HttpContext.User.Claims.SingleOrDefault(o=>o.Type==ClaimTypes.NameIdentifier).Value, myData);
        return RedirectToAction("MyAccount");
    }
    
    [AllowAnonymous]
    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
    
    private string Generate(User user)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JwtSettings:Key"]));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.username),
            // new Claim(ClaimTypes.GivenName, user.Name),
            // new Claim(ClaimTypes.Surname, user.Surname),
            new Claim(ClaimTypes.Role, user.role.ToString())
        };

        var token = new JwtSecurityToken(_config["JwtSettings:Issuer"],
            _config["JwtSettings:Audience"],
            claims,
            expires: DateTime.Now.AddMinutes(15),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
    
}