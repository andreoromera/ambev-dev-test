using Ambev.Dev.Test.Domain.Enums;
using Ambev.Dev.Test.Domain.Models;
using FluentValidation;

namespace Ambev.Dev.Test.Domain.Validation;

public class CreateEmployeeModelValidator : AbstractValidator<CreateEmployeeModel>
{
    public CreateEmployeeModelValidator()
    {
        RuleFor(model => model.FirstName)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage("The employee's first name cannot be empty")
            .MaximumLength(50)
            .WithMessage("The employee's first name cannot be longer than 50 characters");

        RuleFor(model => model.LastName)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage("The employee's last name cannot be empty")
            .MaximumLength(100)
            .WithMessage("The employee's last name cannot be longer than 100 characters");

        RuleFor(model => model.Document)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage("The employee's document number cannot be empty")
            .MaximumLength(20)
            .WithMessage("The employee's document number cannot be longer than 20 characters");

        RuleFor(model => model.BirthDate)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage("The employee's birth date cannot be empty")
            .Must(birthDate => DateTime.TryParse(birthDate, out _))
            .WithMessage("Invalid employee's birth date format")
            .Must(birthDate => DateTime.Parse(birthDate) <= DateTime.Today.AddYears(-18))
            .WithMessage("The employee should be at least 18 years old");

        RuleFor(model => model.Role)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage("The employee's role cannot be empty. Possible values are: Admin, President, Director, Manager, Coordinator, Developer")
            .Must(role => Enum.IsDefined(typeof(Role), role))
            .WithMessage("Invalid role. Possible values are: Admin, President, Director, Manager, Coordinator, Developer");

        RuleFor(model => model.Phones)
            .NotEmpty()
            .WithMessage("The employee must have at least one phone number");

        RuleForEach(model => model.Phones)
            .ChildRules(phone =>
            {
                phone.RuleFor(x => x.Prefix)
                    .Cascade(CascadeMode.Stop)
                    .NotEmpty()
                    .WithMessage("The phone prefix cannot be empty")
                    .MaximumLength(7)
                    .WithMessage("The phone prefix cannot be longer than 7 characters");

                phone.RuleFor(x => x.PhoneNumber)
                    .Cascade(CascadeMode.Stop)
                    .NotEmpty()
                    .WithMessage("The phone number cannot be empty")
                    .MaximumLength(10)
                    .WithMessage("The phone number cannot be longer than 10 characters");

                phone.RuleFor(x => x.PhoneType)
                    .Cascade(CascadeMode.Stop)
                    .NotEmpty()
                    .WithMessage("The phone type cannot be empty. Possible values are: Home, CellPhone, Work")
                    .Must(type => Enum.IsDefined(typeof(PhoneType), type))
                    .WithMessage("Invalid phone type. Possible values are: Home, CellPhone, Work");
            });

        RuleFor(model => model.Email).SetValidator(new EmailValidator());
        RuleFor(model => model.Password).SetValidator(new PasswordValidator());
    }
}
