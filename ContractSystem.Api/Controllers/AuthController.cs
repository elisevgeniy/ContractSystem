using ContractSystem.Core.Models.In;
using ContractSystem.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace ContractSystem.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        UserService _userService;

        public AuthController(UserService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        public ActionResult<string> GetToken(LoginIn loginIn)
        {
            try
            {
                var claims = CreateClaims(loginIn);

                var token = new JwtSecurityToken(
                    issuer: "SecrerIssuer",
                    audience: "App",
                    notBefore: DateTime.Now,
                    claims: claims,
                    expires: DateTime.Now.AddDays(1),
                    signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes("SecretIssuerKeySecretIssuerKeySecretIssuerKey")), SecurityAlgorithms.HmacSha256Signature)
                );

                var tokenStr = new JwtSecurityTokenHandler().WriteToken(token);
                return Ok(tokenStr);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        private List<Claim> CreateClaims(LoginIn loginIn)
        {
            var authed = _userService.Auth(loginIn);

            if (!authed) throw new Exception("Не правильный логин/пароль");

            var user = _userService.GetUser(loginIn.Login);

            return new List<Claim>()
            {
                new Claim(ClaimsIdentity.DefaultRoleClaimType, user.Role.ToString()),
                new Claim(ClaimsIdentity.DefaultNameClaimType, user.Login),
                new Claim("id", user.Id.ToString())
            };
        }
    }
}
