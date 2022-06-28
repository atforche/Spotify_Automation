using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace SpotifyAutomation;

public class Program
{

    public static IConfigurationRoot Configuration { get; set; }

    public static void Main(string[] args)
    {
        var devEnvironmentVariable = Environment.GetEnvironmentVariable("NETCODE_ENVIRONMENT");

        var isDevelopment = string.IsNullOrEmpty(devEnvironmentVariable) ||
            devEnvironmentVariable.ToLower() == "development";

        var builder = new ConfigurationBuilder();
        builder.AddJsonFile("Properties\\appsettings.json", optional: false, reloadOnChange: true);

        if (isDevelopment)
        {
            builder.AddUserSecrets<SpotifyApiConfiguration>();
        }

        Configuration = builder.Build();

        IServiceCollection services = new ServiceCollection();

        services
            .Configure<SpotifyApiConfiguration>(Configuration.GetSection("Spotify"))
            .AddOptions()
            .AddLogging()
            .AddSingleton<ISecretRevealer, SecretRevealer>()
            .BuildServiceProvider();

        var serviceProvider = services.BuildServiceProvider();
        
        var revealer = serviceProvider.GetService<ISecretRevealer>();
        revealer.Reveal();        
    }
}