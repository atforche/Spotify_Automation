using Microsoft.AspNetCore.Mvc;
using NLog;

using SpotifyAutomation;
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
        public ActionResult<AuthorizationCodeResponse?> GetAuthorizationCode(AuthorizationCodeRequest request)
        {
            // Verify that the request object received is valid
            Logger.Info($"Received an Authorization Code Request with state: '{request.State}'");
            if (!AuthorizationCodeRequest.Validate(request, GlobalConstants.State))
            {
                Logger.Error("Received request with incorrect random state");
                return new EmptyResult();
            }

            // Request an authorization code from the Spotify API and return the result to the client
            var response = new AuthorizationCodeResponse("Hello There", request.State);
            Logger.Info($"Sending an Authorization Code Response with state: '{response.State}'");
            return response;
        }
    }
}