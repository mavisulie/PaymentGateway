using Newtonsoft.Json;

namespace PaymentGateway.Api.Requests;

/// <summary>
/// Payment retrieval object.
/// </summary>
public class PaymentRetrievalRequest
    {
        /// <summary>
        /// Guid associated to payment.
        /// </summary>
        [JsonProperty("paymentId")]
        public Guid PaymentId { get; set; }
    }

