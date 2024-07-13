using JadedCms.Models.Auth;
using Microsoft.AspNetCore.Mvc;

namespace JadedCms;

public class AuthController: Controller
{
    public AuthController()
    {

    }

    public IActionResult Login()
    {
        LoginModel model = new LoginModel();
        return View(model);
    }
}
