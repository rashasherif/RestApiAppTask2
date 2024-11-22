using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TestRESTAPI.DTO;
using TestRESTAPI.Models;

namespace TestRESTAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        public AccountController(UserManager<ApplicationUser> userManager , IConfiguration configuration)
        {
            _userManager = userManager;
            this.configration = configuration;

        }
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration configration;

        [HttpPost("Register")]
        public async  Task<IActionResult> RegisterNewUser (dtoNewUser user)
        {

            if (ModelState.IsValid) {

                ApplicationUser applicationUser = new()
                {
                    UserName = user.UserName,
                    Email = user.Email,
                };
              IdentityResult result= await _userManager.CreateAsync(applicationUser ,user.Password);
                if (result.Succeeded)
                {
                    return Ok("succes");
                }
                else {
                    foreach (var item in result.Errors) {

                        ModelState.AddModelError("", item.Description);
                    }
                }
            }


            return BadRequest(ModelState);
        }



        [HttpPost]
        public async Task<IActionResult> LogIn(dtoLogin login)
        {

            if (ModelState.IsValid) { 
            
                ApplicationUser? user = await _userManager.FindByNameAsync(login.UserName);
                if (user != null) {

                    if(await _userManager.CheckPasswordAsync(user, login.Password)) {

                       var claims= new List<Claim>();
                      //  claims.Add(new Claim("tokenNo","70"));
                        claims.Add(new Claim(ClaimTypes.Name, user.UserName));
                        claims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id));

                        claims.Add(new Claim(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));

                        var roles= await _userManager.GetRolesAsync(user);
                        foreach (var role in roles) {

                            claims.Add(new Claim(ClaimTypes.Role, role.ToString()));
                        }
                        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configration["JWT:key"]));
                        var sc = new SigningCredentials(key , SecurityAlgorithms.HmacSha256);
                        var token = new JwtSecurityToken(
                            claims: claims,
                            issuer: configration["JWT:Issuer"],
                            audience: configration["JWT:Audience"],
                            expires: DateTime.Now.AddHours(1),
                            signingCredentials: sc

                            );

                        var _token = new
                        {
                            token = new JwtSecurityTokenHandler().WriteToken(token),    
                            exception = token.ValidTo, 

                        };

                        return Ok(_token);
                    }

                    else { 
                    
                        return Unauthorized();
                    }
                
                }
                else {
                    ModelState.AddModelError("","User Name is invalid");

                }
            }
            return BadRequest(ModelState);

        }






    }
}
