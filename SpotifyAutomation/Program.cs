using NLog;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;

using SpotifyAutomation.Models;

namespace SpotifyAutomation;

public static class Program
{
    /// <summary>
    /// Process object that the Authentication API will be running on
    /// </summary>
    private static Process? apiProcess;

    /// <summary>
    /// HttpClient for making requests to the API
    /// </summary>
    private static HttpClient client = new HttpClient();

    /// <summary>
    /// Configure the logger for this class
    /// </summary>
    private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

    /// <summary>
    /// Initializes the API and runs the Spotify Automation process
    /// </summary>
    /// <param name="args">Command line arguments</param>
    /// <returns>None</returns>
    public async static Task Main(string[] args)
    {
        try
        {
            LaunchAPI();
            GlobalConstants.State = GenerateRandomState(GlobalConstants.StateLength);
            await ConnectToSpotifyApi();
        }
        finally
        {
            CloseAPI();
            LogManager.Shutdown();
        }
    }

    /// <summary>
    /// Opens a separate process to run the API and initializes an HTTPClient to connect to the local API
    /// </summary>
    public static void LaunchAPI()
    {
        // Start up the API
        var startInfo = new ProcessStartInfo();
        startInfo.FileName = $"{GlobalConstants.ApiExeDirectory}\\{GlobalConstants.ApiProjectName}.exe";
        startInfo.WindowStyle = ProcessWindowStyle.Hidden;
        startInfo.CreateNoWindow = true;
        apiProcess = Process.Start(startInfo);

        // Initialize the HTTP client we'll use to connect
        client.BaseAddress = new Uri(GlobalConstants.BaseApiUrl);
        client.DefaultRequestHeaders.Accept.Clear();
        client.DefaultRequestHeaders.Accept.Add(
            new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
        client.Timeout = TimeSpan.FromSeconds(300);
    }

    /// <summary>
    /// Generates a random state string for this application run to use
    /// </summary>
    /// <param name="length">Length of random state string to generate</param>
    /// <returns>The randomly generated state string</returns>
    private static string GenerateRandomState(int length)
    {
        char[] chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890".ToCharArray();
        byte[] data = new byte[4 * length];

        using (var crypto = RandomNumberGenerator.Create())
        {
            crypto.GetBytes(data);
        }

        StringBuilder builder = new StringBuilder();
        for (int i = 0; i < length; i++)
        {
            var randomNumber = BitConverter.ToUInt32(data, i * 4);
            var index = randomNumber % chars.Length;
            builder.Append(chars[index]);
        }

        return builder.ToString();
    }

    /// <summary>
    /// Uses the Authentication API to connect to the Spotify API
    /// </summary>
    public static async Task<int> ConnectToSpotifyApi()
    {
        // Verify that the local API is up and running
        var statusRequest = new StatusRequest();
        StatusResponse? statusResponse = await statusRequest.SendGetRequest(client);
        if (!StatusResponse.Validate(statusResponse))
        {
            var exception = new Exception("Unable to connect to local API");
            Logger.Error(exception);
            throw exception;
        }

        // Send our connection request to the Spotify API
        var authRequest = new AuthorizationCodeRequest();
        AuthorizationCodeResponse? authResponse = await authRequest.SendPostRequest(client);

        // Verify that we got a valid response back from the API
        if (!AuthorizationCodeResponse.Validate(authResponse, GlobalConstants.State))
        {
            var exception = new Exception($"Invalid response received. Request state: {authRequest.State}");
            Logger.Error(exception);
            throw exception;
        }

        // Log our success and store the authorization code
        Logger.Info($"User authorization code received. Response state: {authResponse.State}");
        return 0;
    }

    /// <summary>
    /// Forces the process running the Authenticaation API to close and free all resources
    /// </summary>
    public static void CloseAPI()
    {
        if (apiProcess != null)
        {
            apiProcess.Kill();
        }
    }
}