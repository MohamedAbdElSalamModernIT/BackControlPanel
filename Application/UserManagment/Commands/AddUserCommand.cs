using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.UserManagment.Dto;
using AutoMapper;
using Common;
using Common.Exceptions;
using Domain.Entities;
using Domain.Entities.Auth;
using Domain.Entities.Benaa;
using Domain.Enums;
using Infrastructure.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.UserManagment.Commands
{
    public class AddUserCommand : IRequest<Result>
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string[] Roles { get; set; }
        public bool Active { get; set; }
        public UserType UserType { get; set; }
        public int? AmanaId { get; set; }
        public int? BaladiaId { get; set; }
        public string OfficeName { get; set; }

        class Handler : IRequestHandler<AddUserCommand, Result>
        {

            private readonly IMapper _mapper;
            private readonly IImageService _imageService;
            private readonly IAppDbContext context;
            private readonly UserManager<User> _userManager;

            public Handler(UserManager<User> userManager, IMapper mapper, IImageService imageService, IAppDbContext context)
            {
                _userManager = userManager;
                _mapper = mapper;
                _imageService = imageService;
                this.context = context;
            }
            public async Task<Result> Handle(AddUserCommand request, CancellationToken cancellationToken)
            {

                var user = new AppUser()
                {
                    UserName = request.UserName.ToLower().Trim(),
                    NormalizedUserName = request.UserName.ToUpper(),
                    Email = request.UserName.ToLower().Trim(),
                    NormalizedEmail = request.UserName.ToUpper(),
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    UserType = request.UserType,
                    Active = request.Active
                };

                var result = await _userManager.CreateAsync(user, request.Password);

                if (!result.Succeeded)
                    throw new ApiException(ApiExceptionType.ValidationError, result.Errors.Select(e => new ErrorResult(e.Code, e.Description)).ToArray());


                if (request.Roles.Length > 0)
                    await _userManager.AddToRolesAsync(user, request.Roles);

                if (request.UserType != UserType.Other)
                {
                    var client = new Client()
                    {
                        IdentityId = user.Id,
                    };

                    client.AmanaId = request.AmanaId.HasValue ? request.AmanaId.Value : null;
                    client.BaladiaId = request.BaladiaId.HasValue ? request.BaladiaId.Value : null;
                    client.OfficeName = request.OfficeName;

                    await context.tblClients.AddAsync(client);
                }
                return Result.Successed(_mapper.Map<UserDto>(user));

            }
        }
    }


}
