using PaymentGateway.Api.Client.Responses;
using PaymentGateway.Api.Requests;

namespace PaymentGateway.Api.Client.Interfaces;

/// <summary>
/// Client to manage payments with the acquiring bank.
/// </summary>
public interface ICKOBankClient
{
    /// <summary>
    /// Sends the payment to the acquiring bank so the payment can be processed.
    /// </summary>
    Task<ClientResponse<PaymentResponse>> ProcessPaymentAsync(PaymentCreationRequest request);
}