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
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace AuthApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    
    public class UserJWTController : ControllerBase
    {
        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login(string name, string password) 
        {
            //if(string.IsNullOrEmpty(name) || string.IsNullOrEmpty(password)) 
            //{
            //    return BadRequest();
            //}

            //if(name !="test"|| password != "test") 
            //{
            //    // await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            //    //new Response<string>("empty")
            //    return Unauthorized("noooooooo");
            //}

            User user = new User() { Username = "murad"};
            Role role = new Role() { Name = "Admin" };
            user.Role = role;

           var identity = Authenticate(user);

            var jwt = new JwtSecurityToken(
                    issuer: TokenAuth.ISSUER,
                    audience: TokenAuth.AUDIENCE,
                    notBefore: DateTime.Now,
                    claims: identity.Claims,
                    

                    expires: DateTime.Now.Add(TimeSpan.FromMinutes(TokenAuth.LIFETIME)),
                    signingCredentials: new SigningCredentials(TokenAuth.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            var response = new
            {
                access_token = encodedJwt,
                username = identity.Name
            };

            return Ok(response);

        }


        private ClaimsIdentity Authenticate(User user)
        {
            // создаем один claim
            var claims = new List<Claim>
            {
                new Claim("Name", user.Username),
                new Claim("Music","Metal"),
                new Claim("Env".ToLower(),"local".ToLower()),
               // new Claim("Role", user.Role?.Name)
               new Claim(ClaimTypes.Role, user.Role?.Name)
            };

            HttpContext.Session.SetString("name", user.Username);

            HttpContext.Response.Cookies.Append("name", user.Username);

            // создаем объект ClaimsIdentity
            ClaimsIdentity id = new ClaimsIdentity(claims, "Token", "Name", "Role");


            //HttpContext.SignInAsync(JwtBearerDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
            return id;
        }
    }
}
