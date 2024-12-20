using FluentValidation;
using Stardew_completion_guide_api.Models.Request;

namespace Stardew_completion_guide_api.Validations
{
    public class CreateCategoryRequestValidator : AbstractValidator<CreateCategoryRequest>
    {
        public CreateCategoryRequestValidator ()
        {
            RuleFor(x => x).NotEmpty();
            RuleFor(x => x.Name).NotEmpty();
        }
    }
}
