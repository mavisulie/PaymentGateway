using System.Data.SqlTypes;
using NSubstitute;
using PaymentGateway.Api.Client.Interfaces;
using PaymentGateway.Api.Client.Responses;
using PaymentGateway.Api.Interfaces;
using PaymentGateway.Api.Requests;
using PaymentGateway.Api.Services;
using PaymentGateway.Domain.Entities;

namespace PaymentGateway.Tests.Unit_tests.Services;

[TestFixture]
public class PaymentServiceTests
{
    private PaymentService _sut;
    private ICKOBankClient _client;
    private IPaymentsRepository _paymentsRepo;


    public PaymentServiceTests()
    {
        _client = Substitute.For<ICKOBankClient>();
        _paymentsRepo = Substitute.For<IPaymentsRepository>();
        _sut = new PaymentService(_client, _paymentsRepo);
    }

    [SetUp]
    public void Setup()
    {
        var clientResponse = new ClientResponse<PaymentResponse>()
        {
            Data = new PaymentResponse()
            {
                IsPaymentCompleted = true,
                IsValidCardNumber = true
            },
            Error = null,
            IsSuccess = true
        };
        _client.ProcessPaymentAsync(Arg.Any<PaymentCreationRequest>()).Returns(clientResponse);
    }

    [Test]
    public async Task Should_ReturnSucessfulPayment_When_PaymentIsProcessedByBankAndSavedInDb()
    {
        // Arrange
        var paymentRequest = new PaymentCreationRequest()
        {
            CardNumber = "5253567489036478",
            Amount = 10,
            Currency = "GBP",
            CVV = 243,
            ExpiryDate = DateTime.Now.AddYears(4)
        };

        // Act
        var result = await _sut.ProcessPaymentAsync(paymentRequest);

        // Assert
        Assert.That(result.CardNumber, Is.EqualTo("525XXXXXXXXXXXXX")); // masked card detatils
        Assert.IsNotNull(result.PaymentId);
        Assert.That(result.PaymentStatus, Is.EqualTo("Successful"));
    }

    [Test]
    public async Task Should_ReturnUncessfulPayment_When_PaymentIsNotProcessedByBankAndSavedInDb()
    {
        // Arrange
        var paymentRequest = new PaymentCreationRequest()
        {
            CardNumber = "5253567489036478",
            Amount = 10,
            Currency = "GBP",
            CVV = 243,
            ExpiryDate = DateTime.Now.AddYears(4)
        };

        var clientResponse = new ClientResponse<PaymentResponse>()
        {
            Data = new PaymentResponse()
            {
                IsPaymentCompleted = false,
                IsValidCardNumber = true
            },
            Error = null,
            IsSuccess = false
        };
        _client.ProcessPaymentAsync(Arg.Any<PaymentCreationRequest>()).Returns(clientResponse);

        // Act
        var result = await _sut.ProcessPaymentAsync(paymentRequest);

        // Assert
        Assert.That(result.CardNumber, Is.EqualTo("525XXXXXXXXXXXXX")); // masked card detatils
        Assert.IsNotNull(result.PaymentId);
        Assert.That(result.PaymentStatus, Is.EqualTo("Unsuccessful"));
    }

    [Test]
    public async Task Should_ThrowException_When_PaymentIsNotSavedInDb()
    {
        // Arrange
        var paymentRequest = new PaymentCreationRequest()
        {
            CardNumber = "5253567489036478",
            Amount = 10,
            Currency = "GBP",
            CVV = 243,
            ExpiryDate = DateTime.Now.AddYears(4)
        };
        _paymentsRepo.When(x => 
            x.AddPaymentAsync(Arg.Any<Payment>()))
            .Do((callInfo) => throw new SqlTypeException());

        // Act
        // Assert
        Assert.ThrowsAsync<SqlTypeException>(() => _sut.ProcessPaymentAsync(paymentRequest));
    }
}