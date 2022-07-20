namespace SpotifyAuthenticationWebAPI.Models;

/// <summary>
/// Model representing information needed to connect to and utilize the Spotify API.
/// Populated using .NET User Secrets
/// </summary>
public class StatusRequest
{

	/// <summary>
	/// API Endpoint 
	/// </summary>
	const string endPoint = $"status";

	public StatusRequest() { }

	/// <summary>
	/// Sends a GET request to the API
	/// </summary>
	public async Task<StatusResponse> GetRequest(HttpClient client)
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
}
