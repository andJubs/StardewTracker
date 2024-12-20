using FluentValidation;
using Stardew_completion_guide_api.Models.Request;

namespace Stardew_completion_guide_api.Validations
{
    public class CreateItemRequestValidator: AbstractValidator<CreateItemRequest>
    {
        public CreateItemRequestValidator()
        {
            RuleFor(x => x).NotEmpty();
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.ImageUri).NotEmpty();
        }
    }
}
