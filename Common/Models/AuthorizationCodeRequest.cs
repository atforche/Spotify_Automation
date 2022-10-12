using Microsoft.Extensions.Configuration;
using NLog;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Common.Models;

/// <summary>
/// Model representing information needed to request a user authorization code from the Spotify API.
/// </summary>
public class AuthorizationCodeRequest : BaseModel
{
    /// <summary>
    /// Client ID for our application
    /// </summary>
	[JsonIgnore]
	public string ClientId { get; set; } = "";

	/// <summary>
	/// Local API Endpoint
	/// </summary>
	private static string endPoint = $"/authorize";

	/// <summary>
	/// Spotify API endpoint to request an Authorization Code
	/// </summary>
	public string spotifyEndPoint => $"https://accounts.spotify.com/authorize?response_type={ResponseType}&client_id={ClientId}&scope={Scope}&redirect_uri={RedirectUri}&state={State}";

	/// <summary>
	/// Response type to get from the Spotify API
	/// </summary>
	private static string ResponseType = "code";

	/// <summary>
	/// URL to redirect the User to after they provide login credentials
	/// </summary>
	private static string RedirectUri = $"{GlobalConstants.BaseApiUrl}/authorize/authTokenResponse";

	/// <summary>
	/// Authorization scopes needed on the Spotify API
	/// </summary>
	private static string Scope = "playlist-modify-public";

	/// <summary>
	/// Section of the model in the User Secrets JSON dictionary
	/// </summary>
	private static string ConfigurationKey = "Spotify";

	/// <inheritdoc/>
	[JsonIgnore]
	public override string ValidationErrorMessage => $"Received AuthorizationCodeResponse with the incorrect state. " +
		$"Expected: {GlobalConstants.State}. Received: {State}.";

	/// <summary>
	/// Randomly generated state string to protect against cross-site request forgery
	/// </summary>
	public string State { get; init; }

	/// <summary>
	/// Constructs an AuthenticationCodeRequest object
	/// </summary>
	public AuthorizationCodeRequest()
    {
		// Grab the random state established in the initial handshake
		State = GlobalConstants.State;

		// Get the Client ID and Secret from the local user secrets
		var builder = new ConfigurationBuilder();
		builder.AddUserSecrets<AuthorizationCodeRequest>();
		var configuration = builder.Build();
		ConfigurationBinder.Bind(configuration, ConfigurationKey, this);
    }

	/// <summary>
	/// Sends a POST request to the API endpoint for this request
	/// </summary>
	/// <param name="client"></param>
	/// <returns>a valid AuthorizationCodeResponse with the user authentication code, or null if any error occurs</returns>
	public async Task<AuthorizationCodeResponse> SendPostRequest(HttpClient client)
    {
		// Create the request
		var request = new HttpRequestMessage(HttpMethod.Post, endPoint);

		// Serialize the request object and add it to the request
		var json = JsonSerializer.Serialize(this);
		request.Content = new ByteArrayContent(Encoding.UTF8.GetBytes(json));
		request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

		// Send the request and wait for a response
		Logger.Info($"Sending POST request to /{endPoint}. Request state: {State}");
		HttpResponseMessage response = await client.SendAsync(request);
		if (!response.IsSuccessStatusCode)
        {
			throw new Exception($"Error sending POST request to endpoint {endPoint}. Status Code: {response.StatusCode}. Body: {response.Content.ReadAsStringAsync()}");
        }

		// Read the response object from the HTTP response
		var authResponse = await response.Content.ReadFromJsonAsync(typeof(AuthorizationCodeResponse)) as AuthorizationCodeResponse;
		authResponse.ValidateOrErrorNull();
		return authResponse;
    }

	/// <inheritdoc/>
	protected override bool ValidatePrivate() => State == GlobalConstants.State && ClientId != "";
}
