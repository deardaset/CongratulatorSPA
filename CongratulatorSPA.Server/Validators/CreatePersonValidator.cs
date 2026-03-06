using CongratulatorSPA.Server.Models.Requests;
using FluentValidation;

namespace CongratulatorSPA.Server.Validators
{
    public class CreatePersonValidator : AbstractValidator<CreatePersonRequest>
    {
        const int PhotoMaxLength = 5_000_000;
        const int MaxAge = 110;
        public CreatePersonValidator()
        {            
            RuleFor(x => x.Name)
                .NotEmpty()
                .Matches(@"^[\p{L}\s]+$")
                    .WithMessage("Name must be valid")
                .Length(2, 50)
                    .WithMessage("Name must be longer than 2 symbols and less than 50");
                        
            RuleFor(x => x.BirthDate)
                .Must(BeValidBirthDate)
                .WithMessage("Birthdate must be valid");

            RuleFor(x => x.Email)
                .EmailAddress().WithMessage("Email must be valid")
                .MaximumLength(255).WithMessage("Email is too long")
                .When(x => !string.IsNullOrEmpty(x.Email));

            RuleFor(x => x.Photo)
                .Must(photo => photo == null || photo.ContentType.StartsWith("image/"))
                .WithMessage("Only images allowed");

            RuleFor(x => x.Photo)
                .Must(photo => photo == null || photo.Length <= PhotoMaxLength)
                .WithMessage("Max 5MB");
        }

        private bool BeValidBirthDate(DateTime birthDate)
        {
            return birthDate <= DateTime.Today &&
                   (DateTime.Today.Year - birthDate.Year) <= MaxAge;
        }
    }
}
