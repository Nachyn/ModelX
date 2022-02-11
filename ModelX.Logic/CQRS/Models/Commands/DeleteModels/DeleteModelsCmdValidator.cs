using FluentValidation;
using ModelX.Logic.Common.Validators;

namespace ModelX.Logic.CQRS.Models.Commands.DeleteModels;

public class DeleteModelsCmdValidator : AbstractValidator<DeleteModelsCmd>
{
    public DeleteModelsCmdValidator(IdsCountValidator<DeleteModelsCmd> idsValidator)
    {
        RuleFor(v => v.Ids).SetValidator(idsValidator);
    }
}