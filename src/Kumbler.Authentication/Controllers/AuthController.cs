namespace Kumbler.Authentication.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.Extensions.Options;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authorization;
    using System.Security.Authentication;
    using Kumbler.Authentication.Models;
    using Kumbler.Authentication.Entities;

    [Route("[controller]")]
    [AllowAnonymous]
    public class AuthController : Controller
    {
        private readonly UserManager<KumblerUser> userManager;
        private readonly JwtFactory jwtFactory;
        private readonly JwtOptions jwtOptions;

        public AuthController(UserManager<KumblerUser> userManager, JwtFactory jwtFactory, IOptions<JwtOptions> jwtOptions)
        {
            this.userManager = userManager;
            this.jwtFactory = jwtFactory;
            this.jwtOptions = jwtOptions.Value;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Post([FromBody]RegistrationModel model)
        {
            var userIdentity = new KumblerUser
            {
                Email = model.Email,
                UserName = model.Email
            };

            var result = await this.userManager.CreateAsync(userIdentity, model.Password);

            if (!result.Succeeded) throw new AuthenticationException();

            return new OkResult();
        }

        [HttpPost("login")]
        public async Task<IActionResult> Post([FromBody]LoginModel loginModel)
        {
            var login = await CheckPasswordAsync(loginModel.Email, loginModel.Password);
            if (!login)
            {
                return BadRequest();
            }

            return new OkObjectResult(new
            {
                token = jwtFactory.GenerateEncodedToken(loginModel.Email)
            });
        }

        private async Task<bool> CheckPasswordAsync(string userName, string password)
        {
            if (!string.IsNullOrEmpty(userName) && !string.IsNullOrEmpty(password))
            {
                var userToVerify = await this.userManager.FindByNameAsync(userName);

                if (userToVerify != null)
                {
                    if (await this.userManager.CheckPasswordAsync(userToVerify, password))
                    {
                        return true;
                    }
                }
            }

            return false;
        }
    }
}