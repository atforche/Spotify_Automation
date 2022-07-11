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

    #endregion

    public static void Main(string[] args)
    {
        try
        {
            LaunchAPI();
            Thread.Sleep(10000);
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