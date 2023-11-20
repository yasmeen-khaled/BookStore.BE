using BookStore.BLL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BookStore.APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IConfiguration _configuration;

        public UsersController(UserManager<IdentityUser> userManager,
            IConfiguration configuration)
        {
            _userManager = userManager;
            _configuration = configuration;
        }
        [HttpPost]
        [Route("Register")]
        public ActionResult<TokenDto> Register(RegisterDTo registerDTO)
        {
            var employeeToAdd = new IdentityUser
            {
                UserName = registerDTO.UserName,
                Email = registerDTO.Email
            };
            var result = _userManager.CreateAsync(employeeToAdd, registerDTO.Password).Result;
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, employeeToAdd.Id),
                new Claim(ClaimTypes.Role, "user")
            };

            result = _userManager.AddClaimsAsync(employeeToAdd, claims).Result;
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }
            return NoContent();
        }
        [HttpPost]
        [Route("Register/user")]
        [Authorize(Policy = "allowAdmins")]
        public ActionResult<TokenDto> RegisterUser(RegisterDTo registerDTO)
        {
            var employeeToAdd = new IdentityUser
            {
                UserName = registerDTO.UserName,
                Email = registerDTO.Email
            };
            var result = _userManager.CreateAsync(employeeToAdd, registerDTO.Password).Result;
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, employeeToAdd.Id),
                new Claim(ClaimTypes.Role, registerDTO.Role??"user")
            };

            result = _userManager.AddClaimsAsync(employeeToAdd, claims).Result;
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }
            return NoContent();
        }
        [HttpPost]
        [Route("Login")]
        public async Task<ActionResult<TokenDto>> LogIn(LoginDto loginDto)
        {
            IdentityUser? user = (await _userManager.FindByNameAsync(loginDto.UserName));
            if (user == null)
            {
                return NotFound();
            }
            var isAuthenitcated = _userManager.CheckPasswordAsync(user, loginDto.Password);
            if (!isAuthenitcated.IsCompletedSuccessfully)
            {
                return Unauthorized();
            }
            IList<Claim> claimsList = _userManager.GetClaimsAsync(user).Result;

            var secretKeyString = _configuration.GetValue<string>("SecretKey") ?? string.Empty;
            var secretKeyInBytes = Encoding.ASCII.GetBytes(secretKeyString);
            var secretKey = new SymmetricSecurityKey(secretKeyInBytes);

            //Combination SecretKey, HashingAlgorithm
            var siginingCreedentials = new SigningCredentials(secretKey,
                SecurityAlgorithms.HmacSha256Signature);

            var expiry = DateTime.Now.AddDays(1);

            var token = new JwtSecurityToken(
                claims: claimsList,
                expires: expiry,
                signingCredentials: siginingCreedentials);

            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenString = tokenHandler.WriteToken(token);

            return new TokenDto(tokenString, expiry);
        }
    
    }
}
