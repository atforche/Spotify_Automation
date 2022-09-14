namespace SpotifyAutomation.Models;

/// <summary>
/// Model representing information needed ping the local API
/// </summary>
public class StatusRequest
{
	/// <summary>
	/// API Endpoint 
	/// </summary>
	private const string endPoint = $"/status";

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
	/// Validates that a given request object is valid
	/// </summary>
	/// <returns>Always true in this case</returns>
	public static bool Validate(StatusRequest request, string _) => true;
}
