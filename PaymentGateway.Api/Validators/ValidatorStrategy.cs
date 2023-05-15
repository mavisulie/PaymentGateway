using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using PaymentGateway.Api.Requests;

namespace PaymentGateway.Api.Validators;

/// <summary>
/// Helper class to validate request object.
/// </summary>
public static class ValidatorStrategy
{
    /// <summary>
    /// Validates request object of type <see cref="PaymentCreationRequest"/>
    /// </summary>
    public static bool ValidateRequest(PaymentCreationRequest req, out IActionResult? badRequestResponse)
    {
        badRequestResponse = null;
        var validationResult = new PaymentRequestValidator().Validate(req);

        return SetResponseResult(ref badRequestResponse, validationResult);
    }

    /// <summary>
    /// Validates request object of type <see cref="PaymentRetrievalRequest"/>
    /// </summary>
    public static bool ValidateRequest(PaymentRetrievalRequest req, out IActionResult? badRequestResponse)
    {
        badRequestResponse = null;
        var validationResult = new PaymentRetrievalValidator().Validate(req);

        return SetResponseResult(ref badRequestResponse, validationResult);
    }

    private static bool SetResponseResult(ref IActionResult? badRequestResponse, ValidationResult validationResult)
    {
        if (validationResult.IsValid)
            return true;

        badRequestResponse = new BadRequestObjectResult(validationResult.Errors.Select(e => new
        {
            Field = e.PropertyName,
            Error = e.ErrorMessage
        }));

        return false;
    }
}