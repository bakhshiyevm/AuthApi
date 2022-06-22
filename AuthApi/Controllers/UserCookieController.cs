using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using AuthApi.Models;

namespace AuthApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    
    public class UserCookieController : ControllerBase
    {
        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login(string name, string password) 
        {
            if(string.IsNullOrEmpty(name) || string.IsNullOrEmpty(password)) 
            {
                return BadRequest();
            }

            if(name !="test"|| password != "test") 
            {
                // await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                //new Response<string>("empty")
                return Unauthorized("noooooooo");
            }

            User user = new User() { Username = "murad"};
            Role role = new Role() { Name = "Admin" };
            user.Role = role;

            await Authenticate(user);

            return Ok();

        }

        [HttpPost]
        [Route("Logout")]
        public async Task<IActionResult> Logout()
        {


            if (HttpContext.User.Identity.IsAuthenticated)
            {
                HttpContext.Session.Clear();
                HttpContext.Response.Cookies.Delete("name");
                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);



                return Ok();
            }
            return Problem("Error auth");
        }


        private async Task Authenticate(User user)
        {
            // создаем один claim
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, user.Username),
                new Claim("Music","Metal"),
                new Claim("env","local"),
                new Claim(ClaimsIdentity.DefaultRoleClaimType, user.Role?.Name)
            };

            HttpContext.Session.SetString("name", user.Username);

            HttpContext.Response.Cookies.Append("name", user.Username);

            // создаем объект ClaimsIdentity
            ClaimsIdentity id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
            // установка аутентификационных куки
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
        }
    }
}
