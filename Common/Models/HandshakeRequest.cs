using NLog;
using System.Net.Http.Json;

namespace Common.Models;

/// <summary>
/// Model representing information needed ping the local API
/// </summary>
public class HandshakeRequest
{
	/// <summary>
	/// API Endpoint 
	/// </summary>
	private const string endPoint = $"/handshake";

	/// <summary>
	/// Logger specific to this class
	/// </summary>
	private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

	/// <summary>
	/// Constructs a StatusRequest object
	/// </summary>
	public HandshakeRequest() { }

	/// <summary>
	/// Sends a GET request to the API endpoint for this request
	/// </summary>
	public async Task<HandshakeResponse?> SendGetRequest(HttpClient client)
    {
		try
        {
			// Set the GET request to the local APIs endpoints
			HttpResponseMessage response = await client.GetAsync(endPoint);
			if (!response.IsSuccessStatusCode)
			{
				return null;
			}

			// Read the response object from the HTTP response
			var handshakeResponse = await response.Content.ReadFromJsonAsync(typeof(HandshakeResponse)) as HandshakeResponse;
			return handshakeResponse;
		}
		catch (Exception error)
        {
			Logger.Error(error);
			return null;
        }
    }
}
