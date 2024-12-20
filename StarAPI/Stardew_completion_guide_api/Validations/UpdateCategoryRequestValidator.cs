using FluentValidation;
using Stardew_completion_guide_api.Models.Request;

namespace Stardew_completion_guide_api.Validations
{
    public class UpdateCategoryRequestValidator : AbstractValidator<UpdateItemRequest>
    {
        public UpdateCategoryRequestValidator () {
            RuleFor(x => x.Name).NotEmpty();
        }
    }
}
