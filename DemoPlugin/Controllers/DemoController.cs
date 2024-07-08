using Microsoft.AspNetCore.Mvc;

namespace DemoPlugin.Controllers;

public class DemoController: Controller
{
    public IActionResult Index()
    {
        return View("Index");
    }
}
 