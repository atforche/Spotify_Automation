using Microsoft.AspNetCore.Mvc;
using NLog;
using System.Collections.Concurrent;
using System.Net.Http;

using Common;
using Common.Models;

namespace SpotifyWebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthenticationController : ControllerBase
{
    /// <summary>
    /// Configure the logger for this class
    /// </summary>
    private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

    /// <summary>
    /// Blocking collection that produces authorization code responses as the Spotify API responds
    /// to requests. These responses are then consumed by their waiting GetAuthorizationCode call and
    /// returns to the initial caller
    /// </summary>
    private static readonly BlockingCollection<AuthorizationCodeResponse> responseQueue = new BlockingCollection<AuthorizationCodeResponse>();

    /// <summary>
    /// Constructs an AuthenticationController object
    /// </summary>
    public AuthenticationController() 
    {
        Logger.Info("Authentication Controller initialized successfully");
    }

    /// <summary>
    /// Endpoint to get an authorization code to access the user's account
    /// </summary>
    /// <param name="request">An AuthorizationCodeRequest object containing information about
    /// the authorization request</param>
    /// <returns>An AuthorizationCodeResponse object containing the authorization code or error message</returns>
    [HttpPost("/authorize")]
    public ActionResult<AuthorizationCodeResponse?> GetAuthorizationCode(AuthorizationCodeRequest request)
    {
        // Verify that the request object received is valid
        Logger.Info($"Received an Authorization Code Request with state: '{request.State}'");
        if (!AuthorizationCodeRequest.Validate(request, GlobalConstants.State))
        {
            Logger.Error($"Received request with incorrect random state. Expected: {GlobalConstants.State}. Received: {request.State}.");
            return new EmptyResult();
        }

        // Request an authorization code from the Spotify API
        responseQueue.Add(new AuthorizationCodeResponse(request.State, "Hello"));

        // Await the response from Spotify's API
        var response = responseQueue.Take();
        Logger.Info($"Received an Authorization Code Response with state: '{response.State}'");
        return new AuthorizationCodeResponse(request.State, "Hello");
    }

    /// <summary>
    /// Processes the Spotify's API redirect callback after requesting an authorization token for a user.
    /// If the authorization request has succeeded, the code parameter will contain the authorization token.
    /// If the authorization request has failed, the error parameter will contain a brief summary of the error.
    /// The state parameter should always be populated and validated
    /// </summary>
    [HttpGet("/authorize/authTokenResponse")]
    public void ProcessSpotifyCallback(string? code, string? error, string state)
    {
        if (state != GlobalConstants.State)
        {
            var exception = new Exception($"Invalid response received. Received state: {state}");
            Logger.Error(exception);
            throw exception;
        }

        if (error != null)
        {
            responseQueue.Add(new AuthorizationCodeResponse(state, error: error));
        }
        else if (code != null)
        {
            responseQueue.Add(new AuthorizationCodeResponse(state, userAuthorizationCode: code));
        }
    }
}