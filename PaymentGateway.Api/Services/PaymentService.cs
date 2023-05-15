using PaymentGateway.Api.Client.Interfaces;
using PaymentGateway.Api.Helpers;
using PaymentGateway.Api.Interfaces;
using PaymentGateway.Api.Requests;
using PaymentGateway.Domain.Entities;
using PaymentGateway.Domain.Enums;

namespace PaymentGateway.Api.Services;

/// <summary>
/// Implements <see cref="IPaymentService"/>
/// </summary>
public class PaymentService : IPaymentService
{
    private readonly ICKOBankClient _bankClient;
    private readonly IPaymentsRepository _repo;

    /// <summary>
    /// Initialises <see cref="PaymentService"/>
    /// </summary>
    public PaymentService(ICKOBankClient bankClient, IPaymentsRepository repo)
    {
        _bankClient = bankClient;
        _repo = repo;
    }

    /// <inheritdoc/>
    public async Task<Payment> ProcessPaymentAsync(PaymentCreationRequest request)
    {
        // Process payment
        var paymentProcessing = await _bankClient.ProcessPaymentAsync(request);

        // Mask details and create payment object
        var payment = new Payment()
        {
            PaymentId = Guid.NewGuid(),
            CardNumber = request.CardNumber.Mask(),
            PaymentStatus = paymentProcessing.IsSuccess ? 
                PaymentStatus.Successful.ToString() : PaymentStatus.Unsuccessful.ToString()
        };

        // Save payment
        await _repo.AddPaymentAsync(payment);

        return payment;
    }

    /// <inheritdoc/>
    public async Task<Payment> GetProcessedPayment(Guid paymentId) => 
        await _repo.GetPaymentAsync(paymentId);
    
}