using Microsoft.AspNetCore.Mvc;
using SpotifyAuthenticationWebAPI.Models;

namespace SpotifyAuthenticationWebAPI.Controllers
{
    [ApiController]
    [Route($"api/[controller]")]
    public class StatusController : ControllerBase
    {

        private readonly ILogger<AuthenticationController> _logger;

        public StatusController(ILogger<AuthenticationController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Simple status endpoint to ensure that the local API is up and running
        /// </summary>
        [HttpGet("/status")]
        public void GetServiceStatus() {}
    }
}