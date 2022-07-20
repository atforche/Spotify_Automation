using SpotifyAuthenticationWebAPI.Models;
using System.Diagnostics;

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
    /// Process object that the Authentication API will be running on
    /// </summary>
    private static Process? apiProcess;

    /// <summary>
    /// HttpClient for making requests to the API
    /// </summary>
    private static HttpClient client = new HttpClient();

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
        }
    }


    /// <summary>
    /// Opens a separate process to run the Authentication API and initializes an HTTPClient to connect to the local API
    /// </summary>
    public static void LaunchAPI()
    {
        // Determine the directory that the API .exe is located in
        var projectDirectory = Directory.GetParent(Environment.CurrentDirectory)?.Parent?.Parent?.FullName;
        var apiExeDirectory = $"{projectDirectory}\\{nameof(SpotifyAuthenticationWebAPI)}\\bin\\{Mode}";
        Console.WriteLine(apiExeDirectory);

        // Start up the API
        var startInfo = new ProcessStartInfo();
        startInfo.FileName = $"{apiExeDirectory}\\{nameof(SpotifyAuthenticationWebAPI)}.exe";
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
        StatusResponse response = await statusRequest.GetRequest(client);
        if (!response.Status)
        {
            throw new Exception("Unable to connect to local API");
        }

        // Send our connection request to the API


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