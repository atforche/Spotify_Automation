using Microsoft.AspNetCore.Mvc;

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

        [HttpGet("/status")]
        public string GetServiceStatus()
        {
            return "active";
        }
    }
}