namespace PaymentGateway.Api.Client.Responses;

/// <summary>
/// Acquiring bank response.
/// </summary>
public class PaymentResponse
{
    /// <summary>
    /// Card validation.
    /// </summary>
    public bool IsValidCardNumber { get; set; }

    /// <summary>
    /// Payment status.
    /// </summary>
    public bool IsPaymentCompleted { get; set; }

}