using AutoMapper;
using BookStore.Api.Contracts;
using BookStore.Api.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Api.Controllers
{
    /// <summary>
    /// Controller used to interact with the Users in the book store's database.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UsersController : BaseController
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IConfiguration _configuration;

        public UsersController(ILoggerService logger, 
            IMapper mapper, SignInManager<IdentityUser> signInManager, 
            UserManager<IdentityUser> userManager,
            IConfiguration configuration) 
            : base(logger, mapper)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _configuration = configuration;
        }

        [AllowAnonymous]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Login([FromBody] UserDto userDto)
        {
            try
            {
                var username = userDto.Username;
                var password = userDto.Password;
                var result = await _signInManager.PasswordSignInAsync(username, password, false, false);
                if (result.Succeeded)
                {
                    var user = await _userManager.FindByNameAsync(username);
                    var tokenString = await GenerateJsonWebToken(user);
                    return Ok(new { token = tokenString });
                }
                return Unauthorized(userDto);
            }
            catch (Exception ex)
            {
                return LogErrorAndBuildInternalError(ex, $"Somethig went wrong login with user [{userDto.Username}]. Please contact the Administrator");
            }
        }

        private async Task<string> GenerateJsonWebToken(IdentityUser user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new List<Claim>
            { 
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.Id)
            };
            var roles = await _userManager.GetRolesAsync(user);
            claims.AddRange(roles.Select(r => new Claim(ClaimsIdentity.DefaultRoleClaimType, r)));
            var token = new JwtSecurityToken(
                _configuration["Jwt:Issuer"], 
                _configuration["Jwt:Issuer"], 
                claims, 
                null,
                expires: DateTime.Now.AddHours(6),
                signingCredentials: credentials
            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
