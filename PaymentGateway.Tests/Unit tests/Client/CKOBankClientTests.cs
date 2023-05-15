using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using PaymentGateway.Api.Client;
using PaymentGateway.Api.Client.Responses;
using PaymentGateway.Api.Requests;

namespace PaymentGateway.Tests.Unit_tests.Client;

/// <summary>
/// Unit tests for <see cref="CKOBankClient"/>
/// </summary>
[TestFixture]
public class CKOBankClientTests
{
    private CKOBankClient _sut;
    private MockHttpMessageHandler _messageHandler;
    private HttpClient _client;
    private IHttpClientFactory _clientFactory;

    [SetUp]
    public void Setup()
    {
        var response = new PaymentResponse()
        {
            IsPaymentCompleted = true,
            IsValidCardNumber = true
        };

        _messageHandler = new MockHttpMessageHandler(response);

        _client = new HttpClient(_messageHandler);

        _clientFactory = Substitute.For<IHttpClientFactory>();
        _clientFactory.CreateClient().Returns(_client);

        _sut = new CKOBankClient(_clientFactory);
    }

    [Test]
    public async Task When_ValidPaymentIsDone_Should_ReturnValidResult()
    {
        // Arrange
        var request = new PaymentCreationRequest()
        {
            CardNumber = "5354678394876543",
            Amount = 10,
            Currency = "GBP",
            CVV = 234,
            ExpiryDate = DateTime.Today.AddYears(4)
        };

        // Act
        var result = await _sut.ProcessPaymentAsync(request);

        // Assert
        Assert.That(result.Data.IsPaymentCompleted, Is.EqualTo(true));
        Assert.That(result.Data.IsValidCardNumber, Is.EqualTo(true));
        Assert.That(result.Error, Is.EqualTo(null));
        Assert.That(result.IsSuccess, Is.EqualTo(true));
    }

    [Test]
    public async Task When_InvalidPaymentIsDone_Should_NotReturnValidResult()
    {
        // Arrange
        var request = new PaymentCreationRequest()
        {
            CardNumber = "5354678394876543",
            Amount = 10,
            Currency = "GBP",
            CVV = 234,
            ExpiryDate = DateTime.Today.AddYears(4)
        };

        _messageHandler.ContentToReturn = new PaymentResponse()
        {
            IsPaymentCompleted = false,
            IsValidCardNumber = true
        };

        // Act
        var result = await _sut.ProcessPaymentAsync(request);

        // Assert
        Assert.That(result.Data.IsPaymentCompleted, Is.EqualTo(false));
        Assert.That(result.Data.IsValidCardNumber, Is.EqualTo(true));
        Assert.That(result.Error, Is.EqualTo(null));
        Assert.That(result.IsSuccess, Is.EqualTo(false));
    }

    [Test]
    public async Task When_ExceptionIsThrownDuringPaymentProcessing_Should_ReturnErrorInResponse()
    {
        // Arrange
        var request = new PaymentCreationRequest()
        {
            CardNumber = "5354678394876543",
            Amount = 10,
            Currency = "GBP",
            CVV = 234,
            ExpiryDate = DateTime.Today.AddYears(4)
        };

        _clientFactory.When(x =>
                x.CreateClient())
            .Do((info => throw new Exception()));

        // Act
        var result = await _sut.ProcessPaymentAsync(request);

        // Assert
        Assert.IsNotNull(result.Error);
        Assert.IsNull(result.Data);
        Assert.That(result.IsSuccess, Is.EqualTo(false));
    }
}

public class MockHttpMessageHandler : HttpMessageHandler
{
    public PaymentResponse ContentToReturn { get; set; }

    public MockHttpMessageHandler(PaymentResponse contentToReturn)
    {
        ContentToReturn = contentToReturn;
    }

    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var services = new ServiceCollection()
            .AddMvc()
            .AddWebApiConventions()
            .Services
            .BuildServiceProvider();

        request.Properties.Add(nameof(HttpContext), new DefaultHttpContext
        {
            RequestServices = services
        });

        var response = request.CreateResponse(HttpStatusCode.OK, ContentToReturn);
        return Task.FromResult(response);
    }
}