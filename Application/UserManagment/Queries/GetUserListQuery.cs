using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.UserManagment.Dto;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Common;
using Common.Extensions;
using Common.Infrastructures;
using Domain.Entities.Auth;
using Infrastructure.Interfaces;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.UserManagment.Queries
{
    public class GetUserListQuery : Paging, IRequest<Result>
    {
        public string FullName { get; set; }
        public string Username { get; set; }
        public int BaladiaId { get; set; } = 0;
        public int AmanaId { get; set; } = 0;
        public string OfficeId { get; set; }
        class Handler : IRequestHandler<GetUserListQuery, Result>
        {
            private readonly IAppDbContext _context;
            private readonly IMapper _mapper;
            private readonly IAuditService auditService;

            public Handler(IAppDbContext context, IMapper mapper, IAuditService auditService)
            {
                _context = context;
                _mapper = mapper;
                this.auditService = auditService;
            }

            public async Task<Result> Handle(GetUserListQuery request, CancellationToken cancellationToken)
            {
                var query = _context.AppUsers
                    .Include(e => e.UserRoles)
                    .ThenInclude(e => e.Role)
                    .Protected()
                    .AsQueryable();

                int initailAmanaId = !string.IsNullOrEmpty(auditService.AmanaId) ? int.Parse(auditService.AmanaId) : request.AmanaId;
                int initailBaladiaId = !string.IsNullOrEmpty(auditService.BaladiaId) ? int.Parse(auditService.BaladiaId) : request.BaladiaId;
                var initailOfficeId = !string.IsNullOrEmpty(auditService.OfficeId) ? auditService.OfficeId : request.OfficeId;



                if (initailAmanaId != 0)
                    query = query.Where(e => e.AmanaId == initailAmanaId);

                if (initailBaladiaId != 0)
                    query = query.Where(e => e.BaladiaId == initailBaladiaId);

                if (!string.IsNullOrEmpty(initailOfficeId))
                    query = query.Where(e => e.OfficeId == initailOfficeId);


                if (!string.IsNullOrEmpty(request.FullName))
                    query = query.Where(e => e.FirstName.Contains(request.FullName) || e.LastName.Contains(request.FullName));

                if (!string.IsNullOrEmpty(request.Username))
                    query = query.Where(e => e.UserName.Contains(request.Username));


                var users = await query.ProjectToType<UserDto>()
                    .Where(e=>e.Id != auditService.UserId)
                    .OrderByDescending(e => e.CreatedDate)
                 .ToPagedListAsync(request, cancellationToken);

                return Result.Successed(users);
            }
        }
    }
}
