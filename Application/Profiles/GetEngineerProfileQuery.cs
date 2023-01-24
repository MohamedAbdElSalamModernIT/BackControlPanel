using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.UserManagment.Dto;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Common;
using Common.Exceptions;
using Common.Extensions;
using Common.Infrastructures;
using Domain.Entities.Benaa;
using Domain.Enums;
using Infrastructure.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.UserManagment.Queries
{
    public class GetEngineerProfileQuery : IRequest<Result>
    {
        public string Id { get; set; }
        class Handler : IRequestHandler<GetEngineerProfileQuery, Result>
        {
            private readonly IAppDbContext _context;
            private readonly IMapper _mapper;

            public Handler(IAppDbContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<Result> Handle(GetEngineerProfileQuery request, CancellationToken cancellationToken)
            {
                var user = await _context.AppUsers
                  .Include(e => e.Drawings)
                  .Protected()
                  .FirstOrDefaultAsync(u => u.Id == request.Id, cancellationToken);

                if (user == null) return Result.Failure(ApiExceptionType.NotFound);

                var name = user.FullName;
                var joinDate = user.CreatedDate;
                var drawings = user.Drawings;

                var totalCount = drawings.Count();
                var closedCount = drawings.Count(e => e.OfficeStatus == OfficeDrawingStatus.Closed);
                var assignedCount = drawings.Count(e => e.OfficeStatus == OfficeDrawingStatus.Assigned);
                var inProgressCount = drawings.Count(e => e.OfficeStatus == OfficeDrawingStatus.InProgress);

                var submittedCount = drawings.Count(e => e.Status == DrawingStatus.Submitted);
                var rejectedCount = drawings.Count(e => e.Status == DrawingStatus.Rejected);
                var PendingCount = drawings.Count(e => e.Status == DrawingStatus.Pending);


                var meetDeadlineCount = drawings.Where(e => e.OfficeStatus != OfficeDrawingStatus.Assigned)
                    .Count(e => GetDateDiff(e) >= 0);

                var exceedDeadlineCount = drawings.Where(e => e.OfficeStatus != OfficeDrawingStatus.Assigned)
                    .Count(e => GetDateDiff(e) < 0);

                var values = new
                {
                    name,
                    joinDate,
                    totalCount,
                    closedCount,
                    assignedCount,
                    inProgressCount,
                    PendingCount,
                    rejectedCount,
                    submittedCount,
                    meetDeadlineCount,
                    exceedDeadlineCount
                };

                return Result.Successed(values);
            }
            private int GetDateDiff(Drawing drawing)
            {
                var dueDate = drawing.ActualEndDate ?? DateTime.UtcNow;
                return drawing.PlannedEndDate.Value.Subtract(dueDate).Days;

            }
        }
    }
}