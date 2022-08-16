namespace SpotifyAuthenticationWebAPI.Models;

/// <summary>
/// Model representing information needed to connect to and utilize the Spotify API.
/// Populated using .NET User Secrets
/// </summary>
public class StatusRequest
{
	/// <summary>
	/// Constructs a StatusRequest object
	/// </summary>
	public StatusRequest() { }

	/// <summary>
	/// Sends a GET request to the API endpoint for this request
	/// </summary>
	public async Task<StatusResponse> SendGetRequest(HttpClient client)
    {
		try
        {
			HttpResponseMessage response = await client.GetAsync(endPoint);
			if (!response.IsSuccessStatusCode)
			{
				return new StatusResponse(false);
			}
			return new StatusResponse(true);
		}
		catch
        {
			return new StatusResponse(false);
        }
    }

	/// <summary>
	/// API Endpoint 
	/// </summary>
	private const string endPoint = $"status";
}
