using JadedCms.Models.Auth;
using JadedCmsCore.Interfaces.Database;
using JadedCmsCore.Services.UserManagement;
using Microsoft.AspNetCore.Mvc;

namespace JadedCms;

public class AuthController: Controller
{
    AdminUsers _adminService;
    public AuthController(IDatabaseService dbConfig)
    {
        _adminService = new AdminUsers(dbConfig);
    }

    public IActionResult Login()
    {
        LoginModel model = new LoginModel();
        return View(model);
    }

    public IActionResult LoginCheck(LoginModel model)
    {
        if (ModelState.IsValid)
        {
            JadedEncryption.OnewayEncryption onewayEncryption = new JadedEncryption.OnewayEncryption();
            if(onewayEncryption.VerifyHash(model.Password, "password"))
            {
                //Save login to session
                HttpContext.Session.SetString("username", model.Username);
                return RedirectToAction("Index", "Dashboard");
            }
            else 
            {
                ModelState.AddModelError("Password", "Invalid username or password");
            }
        }
        return View();
    }
}
