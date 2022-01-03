using FluentValidation;
using FluentValidation.Validators;
using Microsoft.Extensions.Localization;
using ModelX.Logic.Common.Extensions;

namespace ModelX.Logic.Common.Validators;

public class IdsCountValidator<T> : PropertyValidator<T, IEnumerable<int>>
{
    private readonly IStringLocalizer<CommonValidatorsResource> _commonLocalizer;

    public IdsCountValidator(IStringLocalizer<CommonValidatorsResource> commonLocalizer)

    {
        _commonLocalizer = commonLocalizer;
    }

    public override string Name => "IdsCountValidator";

    public override bool IsValid(ValidationContext<T> context, IEnumerable<int> idsList)
    {
        return !idsList.IsNullOrEmpty();
    }

    protected override string GetDefaultMessageTemplate(string errorCode)
    {
        return _commonLocalizer["IdsEmpty"];
    }
}