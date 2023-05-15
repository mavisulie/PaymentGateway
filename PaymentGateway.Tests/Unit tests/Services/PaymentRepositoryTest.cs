using Microsoft.EntityFrameworkCore;
using PaymentGateway.Api.Services;
using PaymentGateway.Domain.Entities;
using PaymentGateway.Domain.Enums;

namespace PaymentGateway.Tests.Unit_tests.Services;

[TestFixture]
public class PaymentRepositoryTest
{
    private PaymentRepository _sut;
    private PaymentContext _context;

    public PaymentRepositoryTest()
    {
        _context = new PaymentContext();//Substitute.For<PaymentContext>();
        _context.Payments.Add(new Payment()
        {
            CardNumber = "523XXXXXXXXXXXXX", 
            PaymentId = Guid.NewGuid(),
            PaymentStatus = PaymentStatus.Successful.ToString()
        });

        _sut = new PaymentRepository(_context);
    }

    [Test]
    public async Task Should_SavePaymentInDb_When_DetailsAreCorrect()
    {
        // Arrange
        var payment = new Payment()
        {
            PaymentId = Guid.NewGuid(),
            CardNumber = "542XXXXXXXXXXXXXXXX",
            PaymentStatus = PaymentStatus.Successful.ToString()
        };

        // Act
        await _sut.AddPaymentAsync(payment);

        //Assert
        var insertedPayment = await _context.Payments.FirstOrDefaultAsync(x => x.PaymentId == payment.PaymentId);
        Assert.That(insertedPayment.PaymentId, Is.EqualTo(payment.PaymentId));
        Assert.That(insertedPayment.CardNumber, Is.EqualTo(payment.CardNumber));
        Assert.That(insertedPayment.PaymentStatus, Is.EqualTo(payment.PaymentStatus));
    }

    [Test]
    public async Task Should_GetPayment_When_PaymentWasPreviouslySaved()
    {
        // Arrange
        var id = Guid.NewGuid();
        var payment = new Payment()
        {
            PaymentId = id,
            CardNumber = "542XXXXXXXXXXXXXXXX",
            PaymentStatus = PaymentStatus.Successful.ToString()
        };

        // Act
        await _sut.AddPaymentAsync(payment);
        var retrievedPayment = await _sut.GetPaymentAsync(id);

        //Assert
        Assert.That(retrievedPayment.PaymentId, Is.EqualTo(payment.PaymentId));
        Assert.That(retrievedPayment.CardNumber, Is.EqualTo(payment.CardNumber));
        Assert.That(retrievedPayment.PaymentStatus, Is.EqualTo(payment.PaymentStatus));
    }

    [Test]
    public async Task Should_NotGetPayment_When_PaymentWasNotSavedPreviously()
    {
        // Arrange
        // Act
        var retrievedPayment = await _sut.GetPaymentAsync(Guid.NewGuid());

        //Assert
        Assert.IsNull(retrievedPayment);
    }
}
