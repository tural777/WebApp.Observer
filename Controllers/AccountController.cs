using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebApp.Observer.Events;
using WebApp.Observer.Models;
using WebApp.Observer.Observer;


namespace WebApp.Observer.Controllers;


public class AccountController : Controller
{
    private readonly UserManager<AppUser> _userManager;
    private readonly SignInManager<AppUser> _signInManager;
    private readonly UserObserverSubject _userObserverSubject;
    private readonly IMediator _mediator;

    public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, UserObserverSubject userObserverSubject, IMediator mediator)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _userObserverSubject = userObserverSubject;
        _mediator = mediator;
    }


    public IActionResult Login() => View();


    [HttpPost]
    public async Task<IActionResult> Login(string email, string password)
    {
        var hasUser = await _userManager.FindByEmailAsync(email);

        if (hasUser == null) return View();

        var signInResult = await _signInManager.PasswordSignInAsync(hasUser, password, true, false);

        if (!signInResult.Succeeded)
            return View();

        return RedirectToAction(nameof(HomeController.Index), "Home");
    }


    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();

        return RedirectToAction(nameof(HomeController.Index), "Home");
    }


    public IActionResult SignUp() => View();


    [HttpPost]
    public async Task<IActionResult> SignUp(UserCreateViewModel userCreateViewModel)
    {
        var appUser = new AppUser() { UserName = userCreateViewModel.UserName, Email = userCreateViewModel.Email };

        var identityResult = await _userManager.CreateAsync(appUser, userCreateViewModel.Password);

        if (identityResult.Succeeded)
        {
            await _mediator.Publish(new UserCreatedEvent() { AppUser = appUser });
            //  _userObserverSubject.NotifyObservers(appUser);

            ViewBag.message = "Qeydiyyat işləri uğurla həyata keçdi.";
        }
        else
            ViewBag.message = identityResult.Errors.ToList().First().Description;

        return View();
    }
}