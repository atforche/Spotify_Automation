using Microsoft.Extensions.Configuration;
using SpotifyAutomation.Models;

namespace SpotifyAutomation;

public static class Program
{
    private static IConfigurationRoot? Configuration { get; set; }

    public static void Main(string[] args)
    {
        // Determine if we're in a development environment (should always be true for this project)
        var devEnvironmentVariable = Environment.GetEnvironmentVariable("NETCODE_ENVIRONMENT");
        var isDevelopment = string.IsNullOrEmpty(devEnvironmentVariable) ||
                            devEnvironmentVariable.ToLower() == "development";

        // Set up the configuration and add the User Secrets to the config
        var builder = new ConfigurationBuilder();
        builder.AddJsonFile("Properties\\appsettings.json", optional: false, reloadOnChange: true);
        if (isDevelopment)
        {
            builder.AddUserSecrets<SpotifyApiConfiguration>();
        }
        Configuration = builder.Build();

        // Create and bind an instance of SpotifyApiConfiguration to the Configuration section
        var spotifyApiConfig = new SpotifyApiConfiguration();
        Configuration.GetSection(SpotifyApiConfiguration.Position).Bind(spotifyApiConfig);
        
        // Test printing 
        Console.WriteLine(spotifyApiConfig.ClientId);
    }
}