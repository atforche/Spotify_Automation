using System.Diagnostics.CodeAnalysis;

namespace Common.Models;

/// <summary>
/// Model representing the information that Spotify's API will repond with after requesting a user authorization code
/// </summary>
public class AuthorizationCodeResponse
{
	/// <summary>
	/// String from the Spotify API that can be used to get an access token for the users account
	/// </summary>
	public string? UserAuthorizationCode { get; }

	/// <summary>
	/// String from the Spotify API that specifies what type of error occurred when requesting authorization
	/// </summary>
	public string? Error { get; }

	/// <summary>
	/// Randomly generated state string to protect against cross-site request forgery
	/// </summary>
	public string State { get; }

	/// <summary>
	/// Constructs an AuthorizationCodeResponse object
	/// </summary>
	/// <param name="state">The random state string establish in the initial handshake. Must be verified for security</param>
	/// <param name="userAuthorizationCode">The user authorization code from the API, or null if an error occurred</param>
	/// <param name="error">The error summary received from teh API, or null if no error occurred</param>
	public AuthorizationCodeResponse(string state, string? userAuthorizationCode = null, string? error = null)
	{
		State = state;
		UserAuthorizationCode = userAuthorizationCode;
		Error = error;
	}

	/// <summary>
	/// Validates that a given response object is valid
	/// </summary>
	/// <param name="response">The AuthorizationCodeResponse object returned by the API</param>
	/// <param name="expectedState">The random state string expected by this request</param>
	/// <returns>True if the response object is valid, false otherwise</returns>
	public static bool Validate([NotNullWhen(true)] AuthorizationCodeResponse? response, string expectedState) => response != null && response.State == expectedState;
}
