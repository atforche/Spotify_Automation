using SpotifyAuthenticationWebAPI.Models;
using System.Diagnostics;

namespace SpotifyAutomation;

public static class Program
{

    #region Properties

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

    #endregion

    public async static Task Main(string[] args)
    {
        try
        {
            client.BaseAddress = new Uri("http://localhost:5000/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

            LaunchAPI();
            await ConnectToSpotifyApi();
        }
        finally
        {
            CloseAPI();
        }
    }

    /// <summary>
    /// Opens a separate process to run the Authentication API
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
    }

    /// <summary>
    /// Uses the Authentication API to connect to the Spotify API
    /// </summary>
    public static async Task<int> ConnectToSpotifyApi()
    {
        var statusRequest = new StatusRequest();
        var statusResponse = await statusRequest.PostRequest(client);
        Console.WriteLine(statusResponse);

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