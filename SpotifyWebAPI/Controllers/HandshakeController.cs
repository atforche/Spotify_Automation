using Microsoft.AspNetCore.Mvc;
using NLog;
using System.Security.Cryptography;
using System.Text;

using Common;
using Common.Models;

namespace SpotifyWebAPI.Controllers;

[ApiController]
[Route($"api/[controller]")]
public class HandshakeController : ControllerBase
{
    /// <summary>
    /// Configure the logger for this class
    /// </summary>
    private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

    /// <summary>
    /// Generates a random state string for this application connection to use
    /// </summary>
    /// <param name="length">Length of random state string to generate</param>
    /// <returns>The randomly generated state string</returns>
    private static string GenerateRandomState(int length)
    {
        char[] chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890".ToCharArray();
        byte[] data = new byte[4 * length];

        using (var crypto = RandomNumberGenerator.Create())
        {
            crypto.GetBytes(data);
        }

        StringBuilder builder = new StringBuilder();
        for (int i = 0; i < length; i++)
        {
            var randomNumber = BitConverter.ToUInt32(data, i * 4);
            var index = randomNumber % chars.Length;
            builder.Append(chars[index]);
        }

        return builder.ToString();
    }

    /// <summary>
    /// Constructs a StatusController object
    /// </summary>
    /// <param name="logger"></param>
    public HandshakeController()
    {
        Logger.Info("Status Controller initialized successfully");
    }

    /// <summary>
    /// Simple status endpoint to ensure that the local API is up and running
    /// </summary>
    [HttpGet("/handshake")]
    public ActionResult<HandshakeResponse?> GetServiceStatus() 
    {
        GlobalConstants.State = GenerateRandomState(GlobalConstants.StateLength);
        return new HandshakeResponse(true, GlobalConstants.State);
    }
}