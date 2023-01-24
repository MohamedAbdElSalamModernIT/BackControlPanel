using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Office.Dtos;
using Application.UserManagment.Dto;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Common;
using Common.Exceptions;
using Common.Extensions;
using Common.Infrastructures;
using Domain.Enums;
using Infrastructure.Interfaces;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.UserManagment.Queries
{
    public class GetOfficeProfileQuery : IRequest<Result>
    {
        public string Id { get; set; }
        class Handler : IRequestHandler<GetOfficeProfileQuery, Result>
        {
            private readonly IAppDbContext _context;
            private readonly IMapper _mapper;

            public Handler(IAppDbContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<Result> Handle(GetOfficeProfileQuery request, CancellationToken cancellationToken)
            {
                var office = await _context.tblOffices
                  .Include(e => e.Engineers)
                  .Include(e => e.Amana)
                  .Include(e => e.Drawings)
                  .ThenInclude(e => e.Logs)
                  .Protected()
                  .FirstOrDefaultAsync(u => u.Id == request.Id, cancellationToken);

                if (office == null) return Result.Failure(ApiExceptionType.NotFound);

                var officeDto = office.Adapt<OfficeDto>();
                var drawings = office.Drawings;
                var totalCount = drawings?.Count();
                var logsCount = drawings?.SelectMany(e => e.Logs).Count();
                var baladiatCount = drawings?.GroupBy(e => e.BaladiaId).Count();
                var engineersCount = office.Engineers?.Count() - 1;
                var submittedCount = drawings?.Count(e => e.Status == DrawingStatus.Submitted);
                var rejectedCount = drawings?.Count(e => e.Status == DrawingStatus.Rejected);
                var PendingCount = drawings?.Count(e => e.Status == DrawingStatus.Pending);

                var values = new
                {
                    office = officeDto,
                    totalCount,
                    logsCount,
                    baladiatCount,
                    engineersCount,
                    PendingCount,
                    rejectedCount,
                    submittedCount,
                };

                return Result.Successed(values);
            }

        }
    }
}