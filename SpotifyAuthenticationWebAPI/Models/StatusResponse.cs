using SpotifyAuthenticationWebAPI.Controllers;

namespace SpotifyAuthenticationWebAPI.Models;

/// <summary>
/// Model representing information needed to connect to and utilize the Spotify API.
/// Populated using .NET User Secrets
/// </summary>
public class StatusResponse
{

	#region Properties

	/// <summary>
	/// The Status response from the API server
	/// </summary>
	public string Status { get; set; } = "Not Set";

	#endregion

	public StatusResponse() { }


}
