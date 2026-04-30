using Microsoft.EntityFrameworkCore.Storage;
using Ticketing.Application.Interfaces;
using Ticketing.Domain.Exceptions;
using Ticketing.Infrastructure.Data;

public class UnitOfWork(TicketingDbContext context) : IUnitOfWork
{
    private readonly TicketingDbContext _context = context;
    private IDbContextTransaction? _transaction;

    public async Task BeginTransactionAsync(CancellationToken cancellationToken)
    {
        _transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
    }

    public async Task CommitAsync(CancellationToken cancellationToken)
    {
        try
        {
        await _context.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateConcurrencyException ex)
        {
            throw new ConcurrencyConflictException("El asiento ya ha sido modificado por otro proceso.", ex);
        }
    }

    public async Task RollbackAsync(CancellationToken cancellationToken)
    {
        await _transaction.RollbackAsync(cancellationToken);
    }
}