using EventOrganizerAPI.Models.Dto;
using EventOrganizerAPI.Persistance;
using FluentValidation;

namespace EventOrganizerAPI.Models.Validators
{
    public class RegisterUserDtoValidator : AbstractValidator<RegisterUserDto>
    {

        public RegisterUserDtoValidator(EventOrganizerDbContext dbContext) 
        {
            RuleFor(x => x.DateOfBirth)
                .NotEmpty()
                .WithMessage("Date of birth is required. Please provide your date of birth.");

            RuleFor(x => x.Password).MinimumLength(6);

            RuleFor(x => x.Password).Equal(x => x.ConfirmedPassword);

            RuleFor(x => x.Email)
                .Custom((value, context) =>
            {
                var emailInUse = dbContext.Users.Any(u => u.Email == value);
                if (emailInUse)
                {
                    context.AddFailure("Email", "This email is taken");
                }
            });

        }
    }
}
