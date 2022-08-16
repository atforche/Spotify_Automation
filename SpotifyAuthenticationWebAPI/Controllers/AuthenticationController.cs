using Microsoft.AspNetCore.Mvc;
using SpotifyAuthenticationWebAPI.Models;

namespace SpotifyAuthenticationWebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthenticationController : ControllerBase
    {

        private readonly ILogger<AuthenticationController> _logger;

        public AuthenticationController(ILogger<AuthenticationController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Endpoint to get an authorization code to access the user's account
        /// </summary>
        /// <param name="request">An AuthorizationCodeRequest object containing information about
        /// the authorization request</param>
        /// <returns>An AuthorizationCodeResponse object containing the authorization code or error message</returns>
        [HttpPost("/authorize")]
        public ActionResult<AuthorizationCodeResponse> GetAuthorizationCode(AuthorizationCodeRequest request)
        {
            return new AuthorizationCodeResponse("Hello There", "General Kenobi");
        }
    }
}