using Temelkov_LOGIN_WEB_API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace Temelkov_LOGIN_WEB_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        public static User user = new User();
        private readonly IConfiguration _config;

        public AuthController(IConfiguration config )
        {
            _config = config;
        }

        [HttpPost("login")]
        public async Task<ActionResult<string>> Login(UserDto u)
        {
            user.Username = "test";
            user.Password = "test";
            if (u.Username == "test" && u.Password == "test")
            {
                string correctToken = CreateNewToken(user);
                return Ok(correctToken);
            }
            return default;
        }

        private string CreateNewToken(User u) 
        {
            List<Claim> claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, u.Username),
                new Claim(ClaimTypes.Role, "Admin")
            };
            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_config.GetSection("AppSettings:Token").Value));
            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: cred
                );
            var jwt = new JwtSecurityTokenHandler().WriteToken(token);
            return jwt;
        }
    }
}
