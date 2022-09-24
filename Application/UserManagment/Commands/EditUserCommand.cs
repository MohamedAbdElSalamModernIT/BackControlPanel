using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.UserManagment.Dto;
using AutoMapper;
using Common;
using Common.Exceptions;
using Domain.Entities.Auth;
using Infrastructure.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Application.UserManagment.Commands
{
    public class EditUserCommand : IRequest<Result>
    {
        public string Id { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string[] Roles { get; set; }
        public bool Active { get; set; }
    }

    public class EditUserCommandHandler : IRequestHandler<EditUserCommand, Result>
    {
        private readonly UserManager<User> _userManager;
        private readonly IImageService _imageService;
        private readonly IAppDbContext _context;
        private readonly IMapper _mapper;

        public EditUserCommandHandler(UserManager<User> userManager, IAppDbContext context, IMapper mapper, IImageService imageService)
        {
            _imageService = imageService;
            _userManager = userManager;
            _context = context;
            _mapper = mapper;
        }
        public async Task<Result> Handle(EditUserCommand request, CancellationToken cancellationToken)
        {

            var user = await _context.AppUsers.FirstOrDefaultAsync(u => u.Id == request.Id, cancellationToken);

            user.FirstName = request.FirstName;
            user.LastName = request.LastName;
            user.Active = request.Active;



            if (!String.IsNullOrEmpty(request.Password))
            {
                await _userManager.RemovePasswordAsync(user);
                await _userManager.AddPasswordAsync(user, request.Password);
            }

            var roles = await _userManager.GetRolesAsync(user);

            var result = await _userManager.RemoveFromRolesAsync(user, roles);
            if (!result.Succeeded)
                throw new ApiException(ApiExceptionType.NotFound, result.Errors.Select(s => new ErrorResult(s.Code, s.Description)).ToArray());


            if (request.Roles.Length > 0)
                await _userManager.AddToRolesAsync(user, request.Roles);

            _context.AppUsers.Update(user);
            return Result.Successed(_mapper.Map<UserDto>(user));
        }
    }
}
