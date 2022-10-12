using System.Text.Json.Serialization;

namespace Common.Models;

/// <summary>
/// Model representing response given by local API after ping is sent
/// </summary>
public class HandshakeResponse : BaseModel
{
	/// <inheritdoc/>
	[JsonIgnore]
	public override string ValidationErrorMessage => "Unable to connect to local API";

    /// <summary>
    /// A boolean indicating the status of the local API process
    /// </summary>
    public bool Status { get; set; } = false;

	/// <summary>
	/// Randomly generated state string to protect against cross-site request forgery
	/// </summary>
	public string State { get; init; }

	/// <summary>
	/// Constructs a HandshakeResponse object
	/// </summary>
	/// <param name="success">A boolean indicating whether a connection was successfully established</param>
	/// <param name="state">The random state string establish in the initial handshake. Must be verified for security</param>
	public HandshakeResponse(bool status, string state) 
	{
		Status = status;
		State = state;
	}

	/// <inheritdoc/>
	protected override bool ValidatePrivate() => Status;
}
