namespace PaymentGateway.Api.Client.Responses;

/// <summary>
/// Abstraction of the client response.
/// </summary>
public class ClientResponse<T>
{
    /// <summary>
    /// Client response data.
    /// </summary>
    public T Data { get; set; }

    /// <summary>
    /// Request sucessful result.
    /// </summary>
    public bool IsSuccess { get; set; }

    /// <summary>
    /// Error occurred in the request.
    /// </summary>
    public Exception? Error { get; set; }
}