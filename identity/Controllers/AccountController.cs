using identity.Options;
using identity.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace identity.Controllers;

public class AccountController : Controller
{
    private readonly SignInManager<User> _signInManager;
    private readonly UserManager<User> _userManager;

    public AccountController(
        SignInManager<User> signInManager,
        UserManager<User> userManager)
    {
        _signInManager = signInManager;
        _userManager = userManager;
    }
    public IActionResult Login(string returnUrl) 
        => View(new LoginViewModel() { ReturnUrl = returnUrl});
    
    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        var user = await _userManager.FindByEmailAsync(model.Email);
        var result = await _signInManager.PasswordSignInAsync(user,model.Password, false, false);
        if(!result.Succeeded)
        {
            model.Email = "Error";
        }
        return View(model);
    }
}