namespace SpotifyAutomation;

/// <summary>
/// Static class containing global constants that are shared across projects
/// </summary>
public static class GlobalConstants
{
    /// <summary>
    /// Directory where the APIs executable is located
    /// </summary>
    public static string ApiExeDirectory => $"{ProjectDirectory}\\{ApiProjectName}\\bin\\{Mode}";

    /// <summary>
    /// Name of the API project, used to reference its executable
    /// </summary>
    public static string ApiProjectName = "SpotifyWebAPI";

    /// <summary>
    /// Port on which the API will be listening for responses
    /// </summary>
    public static int BaseApiPort = 5000;

    /// <summary>
    /// Full URL where the API can be reached
    /// </summary>
    public static string BaseApiUrl => $"http://localhost:{BaseApiPort}";

    /// <summary>
    /// Configuration the application is running in. Determines where certain files are located
    /// </summary>
    #if DEBUG
        public static string Mode = "Debug";
    #else
        public static string Mode = "Release";
    #endif

    /// <summary>
    /// Fully qualified path of the base project directory
    /// </summary>
    public static string ProjectDirectory => Directory.GetParent(Environment.CurrentDirectory)?.Parent?.Parent?.FullName ?? throw new Exception("Unable to find base project directory");
}
