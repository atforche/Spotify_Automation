using Microsoft.AspNetCore.Mvc;
using NLog;

namespace SpotifyWebAPI.Controllers;

[ApiController]
[Route($"api/[controller]")]
public class StatusController : ControllerBase
{
    /// <summary>
    /// Configure the logger for this class
    /// </summary>
    private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

	/// <summary>
	/// Constructs a StatusController object
	/// </summary>
	/// <param name="logger"></param>
	public StatusController()
    {
        Logger.Info("Status Controller initialized successfully");
    }

    /// <summary>
    /// Simple status endpoint to ensure that the local API is up and running
    /// </summary>
    [HttpGet("/status")]
    public void GetServiceStatus() {}
}