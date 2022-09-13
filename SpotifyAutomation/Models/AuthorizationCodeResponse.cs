using System.Diagnostics.CodeAnalysis;

namespace SpotifyAutomation.Models;

/// <summary>
/// Model representing the information that Spotify's API will repond with after requesting a user authorization code
/// </summary>
public class AuthorizationCodeResponse
{
	/// <summary>
	/// String from the Spotify API that can be used to get an access token for the users account
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

	/// <summary>
	/// Validates that a given response object is valid
	/// </summary>
	/// <param name="response">The AuthorizationCodeResponse object returns by the API</param>
	/// <param name="expectedState">The random state string expected by this request</param>
	/// <returns>True if the response object is valid, false otherwise</returns>
	public static bool Validate([NotNullWhen(true)] AuthorizationCodeResponse? response, string expectedState) => response != null && response.State == expectedState;
}
