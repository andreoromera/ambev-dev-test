using Ambev.Dev.Test.Domain.Security;
using FluentValidation;
using System.Text.RegularExpressions;

namespace Ambev.Dev.Test.Domain.Validation;

public class CredentialsValidator : AbstractValidator<SignInCredentials>
{
    public CredentialsValidator()
    {
        RuleFor(cred => cred.Email).SetValidator(new EmailValidator());
        RuleFor(cred => cred.Password).SetValidator(new PasswordValidator());
    }
}
