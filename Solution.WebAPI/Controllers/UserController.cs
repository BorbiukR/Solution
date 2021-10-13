using RouteAttribute = Microsoft.AspNetCore.Mvc.RouteAttribute;
using Solution.Contracts.Requests.Authentication;
using Solution.Contracts.Responses.Authentication;
using Solution.Identity.Interfaces;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Solution.WebAPI.Controllers
{
    [Route("api/identity")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        /// <summary>
        /// Register new user
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("register")]
        public async Task<IActionResult> RegisterAsync([FromBody] UserRegistrationRequest model)
        {
            if (!ModelState.IsValid)
                return BadRequest(new AuthFailedResponse
                {
                    Errors = ModelState.Values.SelectMany(x => x.Errors.Select(xx => xx.ErrorMessage))
                });

            var authResponse = await _userService.RegisterUserAsync(model);

            if (!authResponse.Success)
                return BadRequest(new AuthFailedResponse
                {
                    Errors = authResponse.Errors
                });

            return Ok(new AuthSuccessResponse { Token = authResponse.Token });
        }

        /// <summary>
        /// Login to the system
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync([FromBody] UserLoginRequest model)
        {
            if (!ModelState.IsValid)
                return BadRequest(new AuthFailedResponse
                {
                    Errors = ModelState.Values.SelectMany(x => x.Errors.Select(xx => xx.ErrorMessage))
                });

            var authResponse = await _userService.LoginUserAsync(model);

            if (!authResponse.Success)
                return BadRequest(new AuthFailedResponse
                {
                    Errors = authResponse.Errors
                });

            return Ok(new AuthSuccessResponse { Token = authResponse.Token });
        }      
    }
}