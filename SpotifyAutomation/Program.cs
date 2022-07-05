using Microsoft.Extensions.Configuration;
using SpotifyAuthenticationWebAPI.Models;

namespace SpotifyAutomation;

public static class Program
{
    private static IConfigurationRoot? Configuration { get; set; }

    public static void Main(string[] args)
    {
        var authenticationRequest = new SpotifyAuthenticationRequest();
        
        // Test printing 
        Console.WriteLine(authenticationRequest.ClientId);
        Console.WriteLine(authenticationRequest.ClientSecret);
    }
}