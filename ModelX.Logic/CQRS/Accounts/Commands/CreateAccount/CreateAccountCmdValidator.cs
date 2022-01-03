using FluentValidation;
using Microsoft.Extensions.Localization;
using ModelX.Logic.Common.Validators;

namespace ModelX.Logic.CQRS.Accounts.Commands.CreateAccount;

public class CreateAccountCmdValidator : AbstractValidator<CreateAccountCmd>
{
    public CreateAccountCmdValidator(UserNameValidator<CreateAccountCmd> userNameValidator
        , EmailValidator<CreateAccountCmd> emailValidator
        , IStringLocalizer<AccountsResource> accountLocalizer)
    {
        RuleFor(v => v.Email)
            .SetValidator(emailValidator);

        RuleFor(v => v.Username)
            .SetValidator(userNameValidator);

        RuleFor(v => v.Password)
            .Must(p => !string.IsNullOrWhiteSpace(p))
            .WithMessage(accountLocalizer["EnterPassword"]);
    }
}