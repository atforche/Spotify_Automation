using Microsoft.Extensions.Configuration;
using NLog;
using System.Security.Cryptography;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

namespace SpotifyAutomation.Models;

/// <summary>
/// Model representing information needed to request a user authorization code from the Spotify API.
/// ClientID is populated using .NET User Secrets
/// </summary>
public class AuthorizationCodeRequest
{
	/// <summary>
	/// API Endpoint
	/// </summary>
	private const string endPoint = $"authorize";

	/// <summary>
	/// Client ID for our application
	/// </summary>
	private string ClientId { get; set; } = "";

	/// <summary>
	/// Response type to get from the Spotify API
	/// </summary>
	private const string ResponseType = "code";

	/// <summary>
	/// URL to redirect the "User" to after they provide login credentials
	/// </summary>
	private const string RedirectUri = "http://localhost:5000/authentication/authentication_response";

	/// <summary>
	/// Authorization scopes needed on the Spotify API
	/// </summary>
	private const string Scope = "playlist-modify-public";

	/// <summary>
	/// Section of the model in the User Secrets JSON dictionary
	/// </summary>
	private static string ConfigurationSection = "Spotify";

	/// <summary>
	/// Length of the State string provided to the Spotify API
	/// </summary>
	private static int StateLength = 16;

	/// <summary>
	/// Logger specific to this class
	/// </summary>
	private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

	/// <summary>
	/// Generates a random state string for this authentication request to use
	/// </summary>
	private static string GenerateState()
	{
		char[] chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890".ToCharArray();
		byte[] data = new byte[4 * StateLength];

		using (var crypto = RandomNumberGenerator.Create())
		{
			crypto.GetBytes(data);
		}

		StringBuilder builder = new StringBuilder();
		for (int i = 0; i < StateLength; i++)
		{
			var randomNumber = BitConverter.ToUInt32(data, i * 4);
			var index = randomNumber % chars.Length;
			builder.Append(chars[index]);
		}

		return builder.ToString();
	}

	/// <summary>
	/// Randomly generated state string to protect against cross-site request forgery
	/// </summary>
	public string State { get; init; }

	/// <summary>
	/// Constructs an AuthenticationCodeRequest object
	/// </summary>
	public AuthorizationCodeRequest()
    {
		// Generate the random state for this request
		State = GenerateState();

		// Get the Client ID and Secret from the local user secrets
		var builder = new ConfigurationBuilder();
		builder.AddUserSecrets<AuthorizationCodeRequest>();
		var configuration = builder.Build();
		ConfigurationBinder.Bind(configuration, ConfigurationSection, this);
    }

	/// <summary>
	/// Sends a POST request to the API endpoint for this request
	/// </summary>
	/// <param name="client"></param>
	/// <returns>an AuthorizationCodeResponse with the user authentication code, or null if an error occurs</returns>
	public async Task<AuthorizationCodeResponse?> SendPostRequest(HttpClient client)
    {
		try
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
				Logger.Error($"POST request was unsuccessful. \n Error Code: {response.StatusCode} \n Content: {response.Content.ReadAsStringAsync()}");
				return null;
            }

			// Read the response object from the HTTP response
			var authResponse = await response.Content.ReadFromJsonAsync(typeof(AuthorizationCodeResponse)) as AuthorizationCodeResponse;
			return authResponse;

        }
        catch (Exception error)
        {
			Logger.Error(error);
			return null;
        }
    }
}
