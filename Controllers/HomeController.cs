using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WebApp.Observer.Models;


namespace WebApp.Observer.Controllers;


public class HomeController : Controller
{
    public IActionResult Index() => View();

    public IActionResult Privacy() => View();


    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
        => View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
}