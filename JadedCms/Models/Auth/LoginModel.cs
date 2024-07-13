using System.ComponentModel.DataAnnotations;

namespace JadedCms.Models.Auth;

public class LoginModel
{
    [Required]
    public string Username { get; set; }

    [Required]
    public string Password { get; set; }
    public string ReturnUrl { get; set; }
    public bool RememberMe { get; set; }
}
