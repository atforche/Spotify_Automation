namespace SpotifyAutomation.Models;

/// <summary>
/// Model representing information needed to connect to and utilize the Spotify API.
/// Populated using .NET User Secrets
/// </summary>
public class SpotifyApiConfiguration
{
	/// <summary>
	/// Position of the model in the User Secrets Configuration dictionary
	/// </summary>
	public const string Position = "Spotify";

	/// <summary>
	/// Client ID for our application
	/// </summary>
	public string ClientId { get; set; } = "";

	/// <summary>
	/// Client secret for our application
	/// </summary>
	public string ClientSecret { get; set; } = "";
}
