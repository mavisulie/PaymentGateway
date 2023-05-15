using PaymentGateway.Api.Requests;
using PaymentGateway.Domain.Entities;

namespace PaymentGateway.Api.Interfaces;

/// <summary>
/// Abstraction of service to manage payments.
/// </summary>
public interface IPaymentService
{
    /// <summary>
    /// Sends payments to acquiring bank and save details.
    /// </summary>
    Task<Payment> ProcessPaymentAsync(PaymentCreationRequest request);

    /// <summary>
    /// Gets previous payment requests.
    /// </summary>
    Task<Payment> GetProcessedPayment(Guid paymentId);
}
