using Application.Category.Dtos;
using Common;
using Common.Extensions;
using Common.Infrastructures;
using Infrastructure.Interfaces;
using Mapster;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Application.Lookup.Dtos;
using Domain.Enums;
using System;
using System.ComponentModel;
using Common.Attributes;

namespace Application.Lookup.Queries
{
    public class GetEnumsQuery : IRequest<Result>
    {
    }

    public class GetEnumsQueryHandler : IRequestHandler<GetEnumsQuery, Result>
    {
        private readonly IAppDbContext _context;
        public GetEnumsQueryHandler(IAppDbContext context)
        {
            _context = context;
        }

        public async Task<Result> Handle(GetEnumsQuery request, CancellationToken cancellationToken)
        {

            var drawingStatus = Enum.GetValues(typeof(DrawingStatus)).Cast<DrawingStatus>()
                .Select(e => new Value((int)e, e.ToString(), e.GetAttribute<DescriptionAttribute>().Description)).ToList();

            var fileType = Enum.GetValues(typeof(FileType)).Cast<FileType>()
                .Select(e => new Value((int)e, e.ToString(), e.GetAttribute<DescriptionAttribute>().Description)).ToList();
            
            
            var officeDrawingStatus = Enum.GetValues(typeof(OfficeDrawingStatus)).Cast<OfficeDrawingStatus>()
                .Select(e => new Value((int)e, e.ToString(), e.GetAttribute<DescriptionAttribute>().Description)).ToList();


            var conditionStatus = Enum.GetValues(typeof(ConditionStatus)).Cast<ConditionStatus>()
                 .Select(e => new Value((int)e, e.ToString(), e.GetAttribute<DescriptionAttribute>().Description)).ToList();


            var drawingType = Enum.GetValues(typeof(DrawingType)).Cast<DrawingType>()
                 .Select(e => new Value((int)e, e.ToString(), e.GetAttribute<DescriptionAttribute>().Description)).ToList();
            
            var userType = Enum.GetValues(typeof(UserType)).Cast<UserType>()
                 .Select(e => new Value((int)e, e.ToString(),
                 e.GetAttribute<DescriptionAttribute>().Description,
                 e.GetAttribute<ValueAttribute>().Values)).ToList();


            return Result.Successed(new
            {
                drawingStatus,
                userType,
                fileType,
                conditionStatus,
                drawingType,
                officeDrawingStatus
            });

        }



    }


}
