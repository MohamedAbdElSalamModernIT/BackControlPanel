using Application.Auth.Commands;
using FluentValidation;

namespace Application.Validators {
   public class LoginValidator :AbstractValidator<LoginCommand> {

        public LoginValidator() {
            this.RuleFor(r => r.Username).NotNull();
            this.RuleFor(r => r.Password).NotEmpty();
        }

    }
  
}
