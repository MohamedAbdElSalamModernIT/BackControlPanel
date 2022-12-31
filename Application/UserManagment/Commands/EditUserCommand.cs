using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.UserManagment.Dto;
using AutoMapper;
using Common;
using Common.Exceptions;
using Domain.Entities.Auth;
using Domain.Entities.Benaa;
using Domain.Enums;
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
        public UserType UserType { get; set; }
        public int? AmanaId { get; set; }
        public int? BaladiaId { get; set; }
        public string OfficeName { get; set; }
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
            user.UserType = request.UserType;



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

            if (request.UserType != UserType.Other)
            {
                var existedClient = await _context.tblClients.FirstOrDefaultAsync(e => e.IdentityId == request.Id);
                if (existedClient != null) _context.tblClients.Remove(existedClient);

                var client = new Client();
                client.IdentityId = request.Id;
                client.AmanaId = request.AmanaId.HasValue ? request.AmanaId.Value : null;
                client.BaladiaId = request.BaladiaId.HasValue ? request.BaladiaId.Value : null;
                client.OfficeName = request.OfficeName;

                await _context.tblClients.AddAsync(client);
            }

            _context.AppUsers.Update(user);
            return Result.Successed(_mapper.Map<UserDto>(user));
        }
    }
}
