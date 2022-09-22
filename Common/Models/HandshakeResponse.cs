using System.Diagnostics.CodeAnalysis;

namespace Common.Models;

/// <summary>
/// Model representing response given by local API after ping is sent
/// </summary>
public class HandshakeResponse
{
	/// <summary>
	/// A boolean indicating the status of the local API process
	/// </summary>
	public bool Status { get; set; } = false;

	/// <summary>
	/// Randomly generated state string to protect against cross-site request forgery
	/// </summary>
	public string State { get; init; }

	/// <summary>
	/// Constructs a HandshakeResponse object
	/// </summary>
	/// <param name="success">A boolean indicating whether a connection was successfully established</param>
	/// <param name="state">The random state string establish in the initial handshake. Must be verified for security</param>
	public HandshakeResponse(bool status, string state) 
	{
		Status = status;
		State = state;
	}

	/// <summary>
	/// Validates that the given response object is valid
	/// </summary>
	/// <param name="response">The StatusResponse object returns by the API</param>
	/// <returns>True if the response object is valid, false otherwise</returns>
	public static bool Validate([NotNullWhen(true)] HandshakeResponse? response) => response != null && response.Status;
}
