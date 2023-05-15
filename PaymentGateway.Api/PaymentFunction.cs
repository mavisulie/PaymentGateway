using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Extensions;
using Newtonsoft.Json;
using PaymentGateway.Api.Interfaces;
using PaymentGateway.Api.Requests;
using PaymentGateway.Api.Validators;
using PaymentGateway.Domain.Entities;

namespace PaymentGateway.Api;

public class PaymentFunction
{
    private readonly IPaymentService _paymentService;

    public PaymentFunction(IPaymentService paymentService)
    {
        _paymentService = paymentService;
    }

    [Function("ProcessPaymentFunction")]
    [OpenApiOperation(operationId: "Process payment", Summary = "Gets a payment request to be processed.", Description = "Allows the payment processing by the acquiring bank and storing of the payment info.", Visibility = OpenApiVisibilityType.Important)]
    [OpenApiRequestBody(contentType: "application/json", typeof(PaymentCreationRequest))]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(Payment), Summary = "The payment response.", Description = "This returns the payment response.")]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.BadRequest, contentType: "application/json", bodyType: typeof(Payment), Summary = "The validation response.", Description = "This returns the validation response.")]
    public async Task<IActionResult?> ProcessPayment([HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequestData paymentRequest)
    {
        var json = await paymentRequest.ReadAsStringAsync();
        var req = JsonConvert.DeserializeObject<PaymentCreationRequest>(json)
                             ?? throw new ArgumentNullException(nameof(paymentRequest));

        if (!ValidatorStrategy.ValidateRequest(req, out var badRequestResponse))
            return badRequestResponse;

        var payment = await _paymentService.ProcessPaymentAsync(req);

        return new OkObjectResult(payment);
    }

    [Function("GetPaymentFunction")]
    [OpenApiOperation(operationId: "Retrieve payment", Summary = "Retrieves previously processed payment.", Description = "Allows the payment retrieval of a previous payment info.", Visibility = OpenApiVisibilityType.Important)]
    [OpenApiRequestBody(contentType: "application/json", typeof(PaymentRetrievalRequest))]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(Payment), Summary = "The payment response.", Description = "This returns the payment response.")]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.NotFound, contentType: "application/json", bodyType: typeof(object), Summary = "The payment response.", Description = "This returns the payment response.")]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.BadRequest, contentType: "application/json", bodyType: typeof(Payment), Summary = "The validation response.", Description = "This returns the validation response.")]
    public async Task<IActionResult> GetPayment([HttpTrigger(AuthorizationLevel.Anonymous, "post")][FromQuery] HttpRequestData paymentRequestId)
    {
        var json = await paymentRequestId.ReadAsStringAsync();
        var req = JsonConvert.DeserializeObject<PaymentRetrievalRequest>(json)
                  ?? throw new ArgumentNullException(nameof(paymentRequestId));

        if (!ValidatorStrategy.ValidateRequest(req, out var badRequestResponse))
            return badRequestResponse;

        var payment = await _paymentService.GetProcessedPayment(req.PaymentId);

        if (!payment.IsNullOrDefault())
            return new OkObjectResult(payment);

        return new NotFoundObjectResult(req);
    }
}