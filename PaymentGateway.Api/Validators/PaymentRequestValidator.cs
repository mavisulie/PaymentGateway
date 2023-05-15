using FluentValidation;
using PaymentGateway.Api.Requests;

namespace PaymentGateway.Api.Validators
{
    /// <summary>
    /// Payment creation request validator.
    /// </summary>
    public class PaymentRequestValidator : AbstractValidator<PaymentCreationRequest>
    {
        public PaymentRequestValidator()
        {
            RuleFor(x => x.CardNumber).NotEmpty().MaximumLength(16);
            RuleFor(x => x.ExpiryDate).NotEmpty();
            RuleFor(x => x.Amount).NotEmpty().GreaterThan(0);
            RuleFor(x => x.Currency).MaximumLength(3);
            RuleFor(x => x.CVV).NotEmpty();
        }
    }
}