using SpotifyAuthenticationWebAPI.Controllers;
using SpotifyAuthenticationWebAPI;
using System.Text.Json;

namespace SpotifyAuthenticationWebAPI.Models;

/// <summary>
/// Model representing information needed to connect to and utilize the Spotify API.
/// Populated using .NET User Secrets
/// </summary>
public class StatusRequest
{

	#region Properties

	/// <summary>
	/// API Endpoint 
	/// </summary>
	const string endPoint = $"status";

	#endregion

	public StatusRequest() { }

	/// <summary>
	/// Posts the StatusRequest to the API
	/// </summary>
	public async Task<string> PostRequest(HttpClient client)
    {
		HttpResponseMessage response = await client.GetAsync(endPoint);
		response.EnsureSuccessStatusCode();
		string output = await response.Content.ReadAsStringAsync();
		return output;
    }
}
