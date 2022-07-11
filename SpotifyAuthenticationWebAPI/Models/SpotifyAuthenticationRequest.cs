using System.Security.Cryptography;
using System.Text;

namespace SpotifyAuthenticationWebAPI.Models;

/// <summary>
/// Model representing information needed to connect to and utilize the Spotify API.
/// Populated using .NET User Secrets
/// </summary>
public class SpotifyAuthenticationRequest
{

    #region Properties

    /// <summary>
    /// URL to redirect the User to after they provide login credentials
    /// </summary>
    public const string RedirectUri = "https://www.google.com";

	/// <summary>
	/// Authorization scopes needed on the Spotify API
	/// </summary>
	public const string Scopes = "playlist-modify-public";

	/// <summary>
	/// Client ID for our application
	/// </summary>
	public string ClientId { get; set; } = "";

	/// <summary>
	/// Client secret for our application
	/// </summary>
	public string ClientSecret { get; set; } = "";

	/// <summary>
	/// Randomly generated state string to protect against cross-site request forgery
	/// </summary>
	public string State { get; }

	#endregion

	public SpotifyAuthenticationRequest()
    {
		// Create the random state string for this authentication request
		State = GenerateState();

		// Get the Client ID and Secret from the local user secrets
		var builder = new ConfigurationBuilder();
		builder.AddUserSecrets<SpotifyAuthenticationRequest>();
		var configuration = builder.Build();

		configuration.GetSection(ConfigurationPosition).Bind(this);
    }

	/// <summary>
	/// Position of the model in the User Secrets Configuration dictionary
	/// </summary>
	private static string ConfigurationPosition = "Spotify";

	/// <summary>
	/// Length of the State string provided to the Spotify API
	/// </summary>
	private static int StateLength = 16;

	/// <summary>
	/// Generates a random state string for this authentication request to use
	/// </summary>
	private static string GenerateState()
    {
		char[] chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890".ToCharArray();
		byte[] data = new byte[4 * StateLength];

		using (var crypto = RandomNumberGenerator.Create())
        {
			crypto.GetBytes(data);
        }

		StringBuilder builder = new StringBuilder();
		for (int i = 0; i < StateLength; i++)
        {
			var randomNumber = BitConverter.ToUInt32(data, i * 4);
			var index = randomNumber % chars.Length;
			builder.Append(chars[index]);
        }

		return builder.ToString();
	}

}
