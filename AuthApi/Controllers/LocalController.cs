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
using Microsoft.AspNetCore.Authorization;

namespace AuthApi.Controllers
{
    
    [Route("api/[controller]")]
    [ApiController]
    [Authorize("OnlyForMicrosoft")]

    public class LocalController : ControllerBase
    {
        [HttpGet]
        [Route("TestAdmin")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> TestAdmin()
        {


            return Ok();

        }

        [HttpGet]
        [Route("TestUser")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> TestUser()
        {


            return Ok();

        }

        [HttpGet]
        [Route("Test")]
        public async Task<IActionResult> Test() 
        {

             ;

            return Ok(HttpContext.User.FindFirst(x=>x.Type=="Music").Value);

        }

        [AllowAnonymous]
        [HttpGet]
        [Route("AnonymTest")]
        public async Task<IActionResult> AnonymTest()
        {


            return Ok(HttpContext.User.IsInRole("Admin"));

        }


    }
}
