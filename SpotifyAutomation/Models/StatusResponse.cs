
namespace SpotifyAutomation.Models;

/// <summary>
/// Model representing response given by local API after ping is sent
/// </summary>
public class StatusResponse
{
	/// <summary>
	/// The Status response from the API server
	/// </summary>
	public bool Status { get; set; } = false;

	/// <summary>
	/// Constructs a StatusResponse object
	/// </summary>
	/// <param name="success">A boolean indicating whether a connection was successfully established</param>
	public StatusResponse(bool success) 
	{
		Status = success;
	}

	/// <summary>
	/// Validates that the given response object is valid
	/// </summary>
	/// <param name="response">The StatusResponse object returns by the API</param>
	/// <returns>True if the response object is valid, false otherwise</returns>
	public static bool Validate(StatusResponse? response) => response != null && response.Status;
}
