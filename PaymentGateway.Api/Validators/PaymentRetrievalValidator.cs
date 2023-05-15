using FluentValidation;
using PaymentGateway.Api.Requests;

namespace PaymentGateway.Api.Validators
{
    /// <summary>
    /// Payment retrieval validator.
    /// </summary>
    public class PaymentRetrievalValidator : AbstractValidator<PaymentRetrievalRequest>
    {
        public PaymentRetrievalValidator()
        {
            RuleFor(x => x.PaymentId).NotNull().NotEmpty();
        }
    }
}