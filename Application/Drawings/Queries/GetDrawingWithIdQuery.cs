using Application.Drawings.Dto;
using AutoMapper;
using Common;
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

namespace Application.Drawings.Queries
{
    public class GetDrawingWithIdQuery : IRequest<Result>
    {
        public string Id { get; set; }
        class Handler : IRequestHandler<GetDrawingWithIdQuery, Result>
        {
            private readonly IAppDbContext _context;

            public Handler(IAppDbContext context)
            {
                _context = context;

            }

            public async Task<Result> Handle(GetDrawingWithIdQuery request, CancellationToken cancellationToken)
            {
                var drawing =await _context.tblDrawings
               .Include(e => e.Baladia)
               .Include(e => e.BuildingType)
               .Include(e => e.Client)
               .ProjectToType<DrwaingDto>()
               .FirstOrDefaultAsync(e => e.Id == request.Id);


                return Result.Successed(drawing);
            }

        }
    }
}


