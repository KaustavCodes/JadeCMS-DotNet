using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using JadedCms.Models;
using JadedCmsCore.Services.UserManagement;
using JadedCmsCore.Interfaces.Database;

namespace JadedCms.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly AdminUsers _adminUsers;

    public HomeController(ILogger<HomeController> logger, IDatabaseService databaseService)
    {
        _logger = logger;
        _adminUsers = new AdminUsers(databaseService);
    }

    public IActionResult Index()
    {
        var user = _adminUsers.GetAdmins();
        return View();
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
