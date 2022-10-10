using System.Net.Http.Json;

namespace Common.Models;

/// <summary>
/// Model representing information needed ping the local API
/// </summary>
public class HandshakeRequest : BaseModel
{
	/// <summary>
	/// API Endpoint 
	/// </summary>
	private const string endPoint = $"/handshake";

	/// <inheritdoc/>
	public override string ValidationErrorMessage => throw new NotImplementedException();

    /// <summary>
    /// Constructs a StatusRequest object
    /// </summary>
    public HandshakeRequest() { }

	/// <summary>
	/// Sends a GET request to the API endpoint for this request. Throws an exception if the GET
	/// request fails.
	/// </summary>
	public async Task<HandshakeResponse> SendGetRequest(HttpClient client)
    {
		// Set the GET request to the local APIs endpoints
		Logger.Info($"Sending GET request to /{endPoint}.");
		HttpResponseMessage response = await client.GetAsync(endPoint);
		if (!response.IsSuccessStatusCode)
		{
			throw new Exception($"Error sending GET request to endpoint {endPoint}. Status Code: {response.StatusCode}. Body: {response.Content.ReadAsStringAsync()}");
		}

		// Read the response object from the HTTP response
		var handshakeResponse = await response.Content.ReadFromJsonAsync(typeof(HandshakeResponse)) as HandshakeResponse;
		handshakeResponse.ValidateOrErrorNull();
		return handshakeResponse;
    }

	/// <inheritdoc/>
	protected override bool ValidatePrivate() => true;
}
