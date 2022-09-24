using System;
using System.Threading;
using System.Threading.Tasks;
using Common.Interfaces;
using Infrastructure.Interfaces;
using MediatR;

namespace Web.Infrastructures {
  public class DbContextTransactionPipeline<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> {
    private readonly IAppDbContext _context;

    public DbContextTransactionPipeline(IAppDbContext context) {
      _context = context;
    }

    public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next) {
      if (request is IBaseRequest && !(request is IPaging)) {
        var transaction = await _context.CreateTransaction();
        try {
          var response = await next();
          await _context.SaveChangesAsync(cancellationToken);
          await transaction.CommitAsync(cancellationToken);
          return response;
        } catch(Exception) {
          await transaction.RollbackAsync(cancellationToken);
          throw;
        }
      } else {
        var response = await next();
        return response;
      }
    }
  }
}