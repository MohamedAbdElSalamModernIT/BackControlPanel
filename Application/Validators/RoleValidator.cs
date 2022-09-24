using Application.UserManagment.Commands;
using FluentValidation;

namespace Application.Validators {
   public class RoleValidator : AbstractValidator<AddRoleCommand> {

        public RoleValidator() {
            this.RuleFor(r => r.Name).NotEmpty();

        }
    }
}
