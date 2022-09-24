using Application.UserManagment.Commands;
using FluentValidation;

namespace Application.Validators {
   public class UserValidator : AbstractValidator<AddUserCommand> {

        public UserValidator() {
            this.RuleFor(r => r.UserName).NotEmpty();
            this.RuleFor(r => r.Password).NotEmpty();
            this.RuleFor(r => r.Roles).Must(m => m.Length > 0).WithMessage("At least user must assign to one role");

        }


    }
}
