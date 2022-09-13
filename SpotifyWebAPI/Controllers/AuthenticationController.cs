using Microsoft.AspNetCore.Mvc;
using NLog;

using SpotifyAutomation.Models;

namespace SpotifyWebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthenticationController : ControllerBase
    {
        /// <summary>
        /// Configure the logger for this class
        /// </summary>
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Constructs an AuthenticationController object
        /// </summary>
        public AuthenticationController() 
        {
            Logger.Info("Authentication Controller initialized successfully");
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
            Logger.Info($"Received an Authorization Code Request with state: '{request.State}'");

            var response = new AuthorizationCodeResponse("Hello There", request.State);
            Logger.Info($"Sending an Authorization Code Response with state: '{response.State}'");
            return response;
        }
    }
}