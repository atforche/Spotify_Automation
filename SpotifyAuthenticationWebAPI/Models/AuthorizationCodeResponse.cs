using System.Security.Cryptography;
using System.Text;

namespace SpotifyAuthenticationWebAPI.Models;

/// <summary>
/// Model representing the response that Spotify will return to us after getting a user authorization code to an account
/// </summary>
public class AuthorizationCodeResponse
{

	/// <summary>
	/// String from the SpotifyAPI that will allow us to get an access token for the users account
	/// </summary>
	public string UserAuthorizationCode { get; }

	/// <summary>
	/// Randomly generated state string to protect against cross-site request forgery
	/// </summary>
	public string State { get; }

	/// <summary>
	/// Constructs an AuthorizationCodeResponse object
	/// </summary>
	/// <param name="userAuthorizationCode">The user authorization code from the API</param>
	/// <param name="state">The random state string used in the initial request. Must be verified for security</param>
	public AuthorizationCodeResponse(string userAuthorizationCode, string state)
	{
		UserAuthorizationCode = userAuthorizationCode;
		State = state;
	}
}
