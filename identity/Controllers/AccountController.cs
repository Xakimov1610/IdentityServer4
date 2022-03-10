using identity.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace identity.Controllers;

public class AccountController : Controller
{
    public IActionResult Login(string returnUrl) 
        => View(new LoginViewModel() { ReturnUrl = returnUrl});
    
    
}