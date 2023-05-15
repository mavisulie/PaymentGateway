using PaymentGateway.Domain.Entities;

namespace PaymentGateway.Api.Interfaces;

/// <summary>
/// Data repository abstraction for payments.
/// </summary>
public interface IPaymentsRepository
{
    /// <summary>
    /// Saves a payment request.
    /// </summary>
    Task<bool> AddPaymentAsync(Payment payment);

    /// <summary>
    /// Gets a previous payment.
    /// </summary>
    Task<Payment> GetPaymentAsync(Guid paymentId);
}

