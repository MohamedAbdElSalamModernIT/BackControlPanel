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
        public UserType Type { get; set; }
        public int AmanId { get; set; }
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
                    Active = request.Active
                };

                var result = await _userManager.CreateAsync(user, request.Password);

                if (!result.Succeeded)
                    throw new ApiException(ApiExceptionType.ValidationError, result.Errors.Select(e => new ErrorResult(e.Code, e.Description)).ToArray());


                if (request.Roles.Length > 0)
                    await _userManager.AddToRolesAsync(user, request.Roles);

                var client = new Client()
                {
                    IdentityId = user.Id,
                };

                switch (request.Type)
                {
                    case UserType.AmanaManager or UserType.Client:
                        client.AmanaId = request.AmanId;
                        client.OfficeName = request.OfficeName;
                        break;
                    case UserType.BaladiaEmployee:
                        client.BaladiaId = request.AmanId;
                        break;
                }

                await context.tblClients.AddAsync(client);

                return Result.Successed(_mapper.Map<UserDto>(user));

            }
        }
    }


}
