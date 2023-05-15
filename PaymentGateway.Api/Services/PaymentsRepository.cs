using Microsoft.EntityFrameworkCore;
using PaymentGateway.Api.Interfaces;
using PaymentGateway.Domain.Entities;

namespace PaymentGateway.Api.Services;

/// <summary>
/// Implements <see cref="IPaymentsRepository"/>
/// </summary>
public class PaymentRepository : IPaymentsRepository
{
    private readonly PaymentContext _context;

    /// <summary>
    /// Initialises <see cref="PaymentRepository"/>
    /// </summary>
    /// <param name="context"></param>
    public PaymentRepository(PaymentContext context)
    {
        _context = context;
    }

    /// <inheritdoc/>
    public async Task<bool> AddPaymentAsync(Payment payment)
    {
        await _context.Payments.AddAsync(payment);
        return (await _context.SaveChangesAsync() >= 0);
    }

    /// <inheritdoc/>
    public async Task<Payment> GetPaymentAsync(Guid paymentId)
    {
        if (await _context.Payments.AnyAsync(a => a.PaymentId == paymentId))
            return await _context.Payments.FirstOrDefaultAsync(a => a.PaymentId == paymentId);
        return null;
    }
}