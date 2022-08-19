using NLog;
using System.Diagnostics;
using SpotifyAutomation.Models;

namespace SpotifyAutomation;

public static class Program
{
    // Store which configuration the application is running in
    #if DEBUG 
        private static string Mode = "Debug";
    #else
        private static string Mode = "Release";
    #endif

    /// <summary>
    /// Name of the API project, used to reference its executable
    /// </summary>
    private const string ApiProjectName = "SpotifyWebAPI";

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
        // Determine the directory that the API .exe is located in
        var projectDirectory = Directory.GetParent(Environment.CurrentDirectory)?.Parent?.Parent?.FullName;
        var apiExeDirectory = $"{projectDirectory}\\{ApiProjectName}\\bin\\{Mode}";
        Console.WriteLine(apiExeDirectory);

        // Start up the API
        var startInfo = new ProcessStartInfo();
        startInfo.FileName = $"{apiExeDirectory}\\{ApiProjectName}.exe";
        startInfo.WindowStyle = ProcessWindowStyle.Hidden;
        startInfo.CreateNoWindow = true;
        apiProcess = Process.Start(startInfo);

        // Initialize the HTTP client we'll use to connect
        client.BaseAddress = new Uri("http://localhost:5000/");
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
        // Verify that the API is up and running
        var statusRequest = new StatusRequest();
        StatusResponse statusResponse = await statusRequest.SendGetRequest(client);
        if (!statusResponse.Status)
        {
            var exception = new Exception("Unable to connect to local API");
            Logger.Error(exception);
            throw exception;
        }

        // Generate a new random state string for this application run


        // Send our connection request to the API
        var authRequest = new AuthorizationCodeRequest();
        AuthorizationCodeResponse? authResponse = await authRequest.SendPostRequest(client);

        // Verify that we got a valid response back from the API
        if (authResponse == null || authRequest.State != authResponse.State)
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