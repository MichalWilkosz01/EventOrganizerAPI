using EventOrganizerAPI.Models.Dto;
using FluentValidation;

namespace EventOrganizerAPI.Models.Validators
{
    public class CreateEventDtoValidator : AbstractValidator<CreateEventDto>
    {
        public CreateEventDtoValidator()
        {
            RuleFor(x => x.EventStartDate).NotEmpty().WithMessage("EventEndDate field is required.")
                .Must(BeAValidDateTime).WithMessage("EventStartDate field must be a valid date and time (yyyy-mm-dd hh:mm).");

            RuleFor(x => x.EventEndDate).NotEmpty().WithMessage("EventEndDate field is required.")
                .Must(BeAValidDateTime).WithMessage("EventEndDate field must be a valid date and time (yyyy-mm-dd hh:mm).");
        }

        private bool BeAValidDateTime(DateTime dateTime)
        {
            return dateTime.Hour != 0 && dateTime.Minute != 0 && dateTime.Second == 0 && dateTime.Millisecond == 0;
        }
    }
}
