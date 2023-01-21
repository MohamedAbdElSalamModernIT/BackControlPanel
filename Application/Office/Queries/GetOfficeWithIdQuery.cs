using Application.Office.Dtos;
using Common;
using Common.Extensions;
using Common.Infrastructures;
using Infrastructure.Interfaces;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


namespace Application.Office.Queries
{
    public class GetOfficeWithIdQuery : IRequest<Result>
    {
        public string Id { get; set; }
    }

    public class GetOfficeWithIdHandler : IRequestHandler<GetOfficeWithIdQuery, Result>
    {
        private readonly IAppDbContext _context;

        public GetOfficeWithIdHandler(IAppDbContext context)
        {
            _context = context;
        }

        public async Task<Result> Handle(GetOfficeWithIdQuery request, CancellationToken cancellationToken)
        {
            var office = await _context.tblOffices
               .Protected()
               .Include(e => e.Owner)
               .FirstOrDefaultAsync(e => e.Id == request.Id, cancellationToken);

            var officeDto = office.Adapt<OfficeDetailsDto>();

            return Result.Successed(officeDto);
        }
    }
}
