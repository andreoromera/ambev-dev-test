using Ambev.Dev.Test.Domain.Security;
using FluentValidation;

namespace Ambev.Dev.Test.Domain.Validation;

public class SignInCredentialsValidator : AbstractValidator<SignInCredentials>
{
    public SignInCredentialsValidator()
    {
        RuleFor(cred => cred.Email).SetValidator(new EmailValidator());
        RuleFor(cred => cred.Password).SetValidator(new PasswordValidator());
    }
}
