using Common;
using Infrastructure;
using Infrastructure.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Common.Exceptions;

namespace Application.Category.Commands
{
    public class DeleteCategoryCommand : IRequest<Result>
    {
        public string Id { get; set; }
    }
    public class DeleteCategoryHandler : IRequestHandler<DeleteCategoryCommand, Result>
    {
        private readonly IAppDbContext _context;
        public DeleteCategoryHandler(IAppDbContext context)
        {
            _context = context;
        }

        public async Task<Result> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
        {
            var category = await _context.tblCategories
                .FirstOrDefaultAsync(e => e.Id == request.Id);

            if (category == null) return Result.Failure(ApiExceptionType.NotFound);


          
            _context.tblCategories.Remove(category);
            return Result.Successed(category.Id);
        }
    }
}
