using System.Text.RegularExpressions;
using FluentValidation;
using FluentValidation.Validators;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using ModelX.Logic.Common.AppConfigs.Entities;

namespace ModelX.Logic.Common.Validators;

public class EmailValidator<T> : PropertyValidator<T, string>
{
    private const string EmailRegex =
        @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]{2,}\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";

    private readonly AppUserSettings _appUserSettings;

    private readonly IStringLocalizer<CommonValidatorsResource> _commonLocalizer;

    public EmailValidator(IOptions<AppUserSettings> appUserSettings
        , IStringLocalizer<CommonValidatorsResource> commonLocalizer)

    {
        _commonLocalizer = commonLocalizer;
        _appUserSettings = appUserSettings.Value;
    }

    public override string Name => "EmailValidator";

    public override bool IsValid(ValidationContext<T> context, string email)
    {
        return !string.IsNullOrWhiteSpace(email)
               && email.Length <= _appUserSettings.EmailMaxLength
               && Regex.IsMatch(email, EmailRegex);
    }

    protected override string GetDefaultMessageTemplate(string errorCode)
    {
        return _commonLocalizer["EmailInvalid"];
    }
}