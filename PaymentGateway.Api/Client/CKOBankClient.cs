using System.Text;
using Newtonsoft.Json;
using PaymentGateway.Api.Client.Interfaces;
using PaymentGateway.Api.Client.Requests;
using PaymentGateway.Api.Client.Responses;
using PaymentGateway.Api.Requests;

namespace PaymentGateway.Api.Client;

/// <summary>
/// Implements <see cref="ICKOBankClient"/>
/// </summary>
public class CKOBankClient : ICKOBankClient
{
    private const string BaseUrl = "https://ckobank.co.uk/api/";
    private readonly IHttpClientFactory _httpClientFactory;

    /// <summary>
    /// Initializes a new instance of the <see cref="CKOBankClient"/> class.
    /// </summary>
    public CKOBankClient(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    /// <inheritdoc/>
    public async Task<ClientResponse<PaymentResponse>> ProcessPaymentAsync(PaymentCreationRequest request)
    {
        try
        {
            var paymentContent = new BankPaymentRequestModel(
                request.CardNumber,
                request.ExpiryDate,
                request.Amount,
                request.Currency,
                request.CVV);

            var client = _httpClientFactory.CreateClient();
            var response = await client.PostAsync(
                $"{BaseUrl}processPayment",
                new StringContent(JsonConvert.SerializeObject(paymentContent),
                    Encoding.UTF8, "application/json"));

            await response.Content.ReadAsStringAsync();

            response.EnsureSuccessStatusCode();

            var paymentResponse = await response.Content.ReadAsAsync<PaymentResponse>();

            return new ClientResponse<PaymentResponse>()
            {
                Data = paymentResponse,
                Error = null,
                IsSuccess = paymentResponse.IsPaymentCompleted
            };
        }
        catch (Exception ex)
        {
            return new ClientResponse<PaymentResponse>()
            {
                Data = null,
                Error = ex,
                IsSuccess = false
            };
        }
    }
}