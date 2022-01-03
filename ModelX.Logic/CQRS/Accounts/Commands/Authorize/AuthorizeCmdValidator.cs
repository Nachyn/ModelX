using FluentValidation;
using Microsoft.Extensions.Localization;
using ModelX.Domain.Entities;
using ModelX.Logic.Common.Validators;

namespace ModelX.Logic.CQRS.Accounts.Commands.Authorize;

public class AuthorizeCommandValidator : AbstractValidator<AuthorizeCmd>
{
    public AuthorizeCommandValidator(
        IStringLocalizer<AccountsResource> accountLocalizer
        , EmailValidator<AuthorizeCmd> emailValidator)
    {
        RuleFor(v => v.Type)
            .Must(BeValidType)
            .WithMessage(accountLocalizer["LoginType"
                , Token.TypePassword
                , Token.TypeRefresh]);

        RuleFor(v => v.Email)
            .SetValidator(emailValidator);

        var enterPassword = accountLocalizer["EnterPassword"];
        RuleFor(v => v.Password)
            .Must(p => !string.IsNullOrWhiteSpace(p))
            .When(c => !string.IsNullOrWhiteSpace(c.Type)
                       && c.Type.Equals(Token.TypePassword
                           , StringComparison.OrdinalIgnoreCase))
            .WithMessage(enterPassword);

        var enterRefreshToken = accountLocalizer["EnterRefreshToken"];
        RuleFor(v => v.RefreshToken)
            .Must(p => !string.IsNullOrWhiteSpace(p))
            .When(c => !string.IsNullOrWhiteSpace(c.Type)
                       && c.Type.Equals(Token.TypeRefresh
                           , StringComparison.OrdinalIgnoreCase))
            .WithMessage(enterRefreshToken);
    }

    public bool BeValidType(string type)
    {
        return !string.IsNullOrWhiteSpace(type)
               && (type.Equals(Token.TypePassword, StringComparison.OrdinalIgnoreCase)
                   || type.Equals(Token.TypeRefresh,
                       StringComparison.OrdinalIgnoreCase));
    }
}