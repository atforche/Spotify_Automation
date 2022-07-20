using SpotifyAuthenticationWebAPI.Controllers;

namespace SpotifyAuthenticationWebAPI.Models;

/// <summary>
/// Model representing information needed to connect to and utilize the Spotify API.
/// Populated using .NET User Secrets
/// </summary>
public class StatusResponse
{
	/// <summary>
	/// The Status response from the API server
	/// </summary>
	public bool Status { get; set; } = false;

	public StatusResponse(bool success) 
	{
		Status = success;
	}
}
