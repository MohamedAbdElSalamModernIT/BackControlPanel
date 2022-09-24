using System.Threading;
using System.Threading.Tasks;
using Application.UserManagment.Dto;
using AutoMapper;
using Common;
using Common.Exceptions;
using Domain.Entities.Auth;
using Infrastructure.Interfaces;
using MediatR;

namespace Application.UserManagment.Commands {
   public class DeleteUserCommand : IRequest<Result> {

        public string Id { get; set; }
    }

    public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, Result> {
        private readonly IAppDbContext _context;
        private readonly IMapper _mapper;


        public DeleteUserCommandHandler(IAppDbContext context, IMapper mapper) {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Result> Handle(DeleteUserCommand request, CancellationToken cancellationToken) {

            var user = await _context.AppUsers.FindAsync(request.Id);
            if (user == null){
                throw new ApiException(ApiExceptionType.NotFound);
            }
            _context.AppUsers.Remove(user);
            return Result.Successed(_mapper.Map<UserDto>(user));
        }

       
    }
}
