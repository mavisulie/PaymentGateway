using Newtonsoft.Json;

namespace PaymentGateway.Api.Client.Requests;

/// <summary>
/// Request object sent to the acquiring bank.
/// </summary>
public class BankPaymentRequestModel
{
    /// <summary>
    /// Initializes a new instance of the <see cref="BankPaymentRequestModel"/> class.
    /// </summary>
    public BankPaymentRequestModel(string cardNumber, DateTime expiryDate, decimal amount, string currency, int cvv)
    {
        CardNumber = cardNumber;
        ExpiryDate = expiryDate;
        Amount = amount;
        Curency = currency;
        CVV = cvv;
    }

    /// <summary>
    /// User card number.
    /// </summary>
    [JsonProperty("cardNumber")]
    public string CardNumber { get; set; }

    /// <summary>
    /// Card expiry date.
    /// </summary>
    [JsonProperty("expiryDate")]
    public DateTime ExpiryDate { get; set; }

    /// <summary>
    /// Payment amount.
    /// </summary>
    [JsonProperty("amount")]
    public decimal Amount { get; set; }

    /// <summary>
    /// Payment currency.
    /// </summary>
    [JsonProperty("currency")]
    public string Curency { get; set; }

    /// <summary>
    /// Card CVV.
    /// </summary>
    [JsonProperty("cvv")]
    public int CVV { get; set; }
}