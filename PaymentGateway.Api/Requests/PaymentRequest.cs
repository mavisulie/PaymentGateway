using Newtonsoft.Json;

namespace PaymentGateway.Api.Requests;

/// <summary>
/// Payment creation object.
/// </summary>
public class PaymentCreationRequest
{
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
    public string Currency { get; set; }

    /// <summary>
    /// Card CVV.
    /// </summary>
    [JsonProperty("cvv")]
    public int CVV { get; set; }
}