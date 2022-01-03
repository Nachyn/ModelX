using System.Text.RegularExpressions;
using FluentValidation;
using FluentValidation.Validators;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using ModelX.Logic.Common.AppConfigs.Entities;

namespace ModelX.Logic.Common.Validators;

public class UserNameValidator<T> : PropertyValidator<T, string>
{
    private readonly AppUserSettings _appUserSettings;

    private readonly IStringLocalizer<CommonValidatorsResource> _commonLocalizer;

    public UserNameValidator(IStringLocalizer<CommonValidatorsResource> commonLocalizer
        , IOptions<AppUserSettings> appUserOptions)
    {
        _commonLocalizer = commonLocalizer;
        _appUserSettings = appUserOptions.Value;
    }

    public override string Name => "UserNameValidator";

    public override bool IsValid(ValidationContext<T> context, string username)
    {
        return !string.IsNullOrWhiteSpace(username)
               && username.Length >= _appUserSettings.UsernameMinLength
               && username.Length <= _appUserSettings.UsernameMaxLength
               && Regex.IsMatch(username, _appUserSettings.UsernameRegex);
    }

    protected override string GetDefaultMessageTemplate(string errorCode)
    {
        return _commonLocalizer["UsernameInvalid"
            , _appUserSettings.UsernameMinLength
            , _appUserSettings.UsernameMaxLength];
    }
}