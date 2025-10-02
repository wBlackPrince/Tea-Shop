using FluentValidation;
using Users.Contracts;
using Users.Contracts.Dtos;
using Users.Domain;

namespace Users.Application.Commands.CreateUserCommand;

public class CreateUserValidator: AbstractValidator<CreateUserRequestDto>
{
    public CreateUserValidator()
    {
        this.RuleFor(u => u.Password)
            .NotEmpty().WithMessage("Password is required")
            .NotNull().WithMessage("Password is required");

        this.RuleFor(u => u.FirstName)
            .NotEmpty().WithMessage("First Name is required")
            .NotNull().WithMessage("First Name is required");

        this.RuleFor(u => u.LastName)
            .NotEmpty().WithMessage("Last Name is required")
            .NotNull().WithMessage("Last Name is required");

        this.RuleFor(u => u.Email)
            .NotEmpty().WithMessage("Email is required")
            .NotNull().WithMessage("Email is required");

        this.RuleFor(u => u.PhoneNumber)
            .NotEmpty().WithMessage("Phone Number is required")
            .NotNull().WithMessage("Phone Number is required");

        this.RuleFor(u => u.Role)
            .NotEmpty().WithMessage("Role is required")
            .NotNull().WithMessage("Role is required")
            .Must(BeValidRole).WithMessage("Role is not valid");
    }

    private bool BeValidRole(string role)
    {
        return Enum.TryParse(typeof(Role), role, out _);
    }
}