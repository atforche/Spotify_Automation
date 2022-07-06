using Microsoft.Extensions.Configuration;
using SpotifyAuthenticationWebAPI.Models;

namespace SpotifyAutomation;

public static class Program
{ 

    public static void Main(string[] args)
    {
        var authenticationRequest = new SpotifyAuthenticationRequest();
        
        // Test printing 
        Console.WriteLine(authenticationRequest.ClientId);
        Console.WriteLine(authenticationRequest.ClientSecret);

        var workingDirectory = Environment.CurrentDirectory;
        var projectDirectory = Directory.GetParent(workingDirectory)?.Parent?.Parent?.FullName;
        Console.WriteLine(projectDirectory);
    }
}