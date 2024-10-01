using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Ticket_Match.Data;
using Ticket_Match.Models;
using Ticket_Match.Request;

namespace Ticket_Match.Controllers
{
    public class AccountController : Controller
    {
        AppDbContext _db = new AppDbContext();
        [HttpGet]
        public IActionResult RegisterUser() 
        { 
            return View(); 
        }
        [HttpPost]
        public IActionResult RegisterUser(CreateUserRequest request)
        {
            if (ModelState.IsValid is false)
            {
                return View(request);
            }
            var foundUserE =_db.Users.FirstOrDefault(x=>x.Email == request.Email);
            if (foundUserE != null)
            {
                ModelState.AddModelError("Email", "There is already a User with the same Email");
                return View(request);
            }
            var foundUserP = _db.Users.FirstOrDefault(x => x.Phone == request.Phone);
            if (foundUserP != null)
            {
                ModelState.AddModelError("Phone", "There is already a User with the same phone");
                return View(request);
            }
            var user = new User
            {
                Name=request.Name,
                Email=request.Email,
                Phone=request.Phone,
                Password=request.Password,
                Role=Role.User,
            };
            _db.Users.Add(user);
            _db.SaveChanges();
            return RedirectToAction("Index","Home");
        }
        [HttpGet]
        public IActionResult LoginUser()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> LoginUser(LoginRequest request)
        {
            if ( ModelState.IsValid is false)
            {
                return View(request);
            }
            var user = _db.Users.FirstOrDefault(x=>x.Email == request.Email&&x.Password==request.Password);
            if (user is null)
            {
                ModelState.AddModelError("", "Invalid Email or password");
                return View(request);
            }

            List<Claim> claims = [
                new Claim (ClaimTypes.Role,user.Role.ToString()),
                new Claim(ClaimTypes.Sid,user.UserId.ToString()),
                new Claim(ClaimTypes.Email,user.Email),
                ];
            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var Principal = new ClaimsPrincipal(identity);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, Principal);
            return RedirectToAction("Index", "Home");
        }
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

    }
}
