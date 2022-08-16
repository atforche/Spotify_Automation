using System.Security.Cryptography;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace SpotifyAuthenticationWebAPI.Models;

/// <summary>
/// Model representing information needed to connect to and utilize the Spotify API.
/// ClientID and Client Secret are populated using .NET User Secrets
/// </summary>
public class AuthorizationCodeRequest
{
	/// <summary>
	/// Constructs an AuthenticationRequest object
	/// </summary>
	public AuthorizationCodeRequest()
    {
		// Create the random state string for this authentication request
		State = GenerateState();

		// Get the Client ID and Secret from the local user secrets
		var builder = new ConfigurationBuilder();
		builder.AddUserSecrets<AuthorizationCodeRequest>();
		var configuration = builder.Build();
		configuration.GetSection(ConfigurationPosition).Bind(this);
    }

	/// <summary>
	/// Sends a GET request to the API endpoint for this request
	/// </summary>
	/// <param name="client"></param>
	/// <returns></returns>
	public async Task<AuthorizationCodeResponse> SendPostRequest(HttpClient client)
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
			HttpResponseMessage response = await client.SendAsync(request);
			if (!response.IsSuccessStatusCode)
            {
				return new AuthorizationCodeResponse(response.StatusCode.ToString(), 
					await response.Content.ReadAsStringAsync());
            }

			// Read the response object from the HTTP response
			var authResponse = await response.Content.ReadFromJsonAsync(typeof(AuthorizationCodeResponse)) as AuthorizationCodeResponse;

			// Verify that the response came through correctly
			if (authResponse == null)
            {
				return new AuthorizationCodeResponse(response.StatusCode.ToString(),
					await response.Content.ReadAsStringAsync());
            }
			return authResponse;

        }
        catch (Exception e)
        {
			// TODO: add some code here to handle errors and return info to the caller (really not sure where we'd hit this case)
			return new AuthorizationCodeResponse("Something's", "Very broken");
        }
    }

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
	/// URL to redirect the User to after they provide login credentials
	/// </summary>
	private const string RedirectUri = "http://localhost:5000/authentication/authentication_response";

	/// <summary>
	/// Randomly generated state string to protect against cross-site request forgery
	/// </summary>
	public string State { get; }

	/// <summary>
	/// Authorization scopes needed on the Spotify API
	/// </summary>
	private const string Scope = "playlist-modify-public";

	/// <summary>
	/// Position of the model in the User Secrets Configuration dictionary
	/// </summary>
	private static string ConfigurationPosition = "Spotify";

	/// <summary>
	/// Length of the State string provided to the Spotify API
	/// </summary>
	private static int StateLength = 16;

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

}
