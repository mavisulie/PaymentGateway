namespace PaymentGateway.Domain.Entities;

/// <summary>
/// Payment entity.
/// </summary>
public class Payment
{
    /// <summary>
    /// Generated GUID for requested payment.
    /// </summary>
    public Guid PaymentId { get; set; }

    /// <summary>
    /// Masked card number.
    /// </summary>
    public string CardNumber { get; set; }

    /// <summary>
    /// Payment status based on acquiring bank payment request status.
    /// </summary>
    public string PaymentStatus { get; set; }
}