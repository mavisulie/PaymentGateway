namespace PaymentGateway.Api.Client.Responses;

/// <summary>
/// Summarises the client object result
/// </summary>
public class ClientError
{
    /// <summary>
    /// Initialises a new instance of the <see cref="ClientError"/> class.
    /// </summary>
    public ClientError(string errorCode, string message, Exception innerException)
    {
        Exception = innerException;
        ErrorCode = errorCode;
        Message = message;
    }

    /// <summary>
    /// Error code caused on the client side.
    /// </summary>
    public string ErrorCode { get; set; }

    /// <summary>
    /// Error Message.
    /// </summary>
    public string Message { get; set; }

    /// <summary>
    /// Exception thrown.
    /// </summary>
    public Exception Exception { get; set; }
}