using PaymentGateway.Api.Client.Interfaces;
using PaymentGateway.Api.Client.Responses;
using PaymentGateway.Api.Requests;

namespace PaymentGateway.Api.Client;

/// <summary>
/// Implemens <see cref="ICKOBankClient"/>
/// </summary>
public class CKOBankClientMock : ICKOBankClient
{
    private readonly IHttpClientFactory _httpClientFactory;

    /// <summary>
    /// Initializes a new instance of the <see cref="CKOBankClientMock"/> class.
    /// </summary>
    public CKOBankClientMock(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    /// <inheritdoc/>
    public async Task<ClientResponse<PaymentResponse>> ProcessPaymentAsync(PaymentCreationRequest request)
    {
        return new ClientResponse<PaymentResponse>()
        {
            Data = new PaymentResponse()
            {
                IsPaymentCompleted = true,
                IsValidCardNumber = true
            },
            Error = null,
            IsSuccess = true
        };
    }
}