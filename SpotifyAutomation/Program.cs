using NLog;
using System.Diagnostics;

using Common;
using Common.Models;

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
    /// Uses the Authentication API to connect to the Spotify API
    /// </summary>
    public static async Task<int> ConnectToSpotifyApi()
    {
        // Verify that the local API is up and running and save the random state from the API
        var statusRequest = new HandshakeRequest();
        HandshakeResponse handshakeResponse = await statusRequest.SendGetRequest(client);
        GlobalConstants.State = handshakeResponse.State;

        // Send our connection request to the Spotify API
        var authRequest = new AuthorizationCodeRequest();
        AuthorizationCodeResponse? authResponse = await authRequest.SendPostRequest(client);

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