using FluentValidation;
using Stardew_completion_guide_api.Models.Request;

namespace Stardew_completion_guide_api.Validations
{
    public class UpdateItemRequestValidator : AbstractValidator<UpdateItemRequest>
    {
        public UpdateItemRequestValidator() {
            RuleFor(x => x.Name).NotEmpty();
        }
    }
}
