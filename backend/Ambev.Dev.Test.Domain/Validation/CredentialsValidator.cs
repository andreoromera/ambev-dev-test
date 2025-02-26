using Ambev.Dev.Test.Domain.Auth;
using FluentValidation;
using System.Text.RegularExpressions;

namespace Ambev.Dev.Test.Domain.Validation;

public class CredentialsValidator : AbstractValidator<Credentials>
{
    public CredentialsValidator()
    {
        RuleFor(cred => cred.Email).SetValidator(new EmailValidator());
        RuleFor(cred => cred.Password).SetValidator(new PasswordValidator());
    }
}
