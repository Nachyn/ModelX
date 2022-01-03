using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using ModelX.Domain.Entities;
using ModelX.Logic.Common.AppConfigs.Main;
using ModelX.Logic.Common.Exceptions.Api;
using ModelX.Logic.Common.Exceptions.Base;
using ModelX.Logic.Common.ExternalServices.Database;
using ModelX.Logic.Common.ExternalServices.DateTimeService;
using ModelX.Logic.CQRS.Tokens.Commands.CreateAccessToken;
using ModelX.Logic.CQRS.Tokens.Commands.CreateRefreshToken;

namespace ModelX.Logic.CQRS.Accounts.Commands.Authorize;

public record AuthorizeCmd : IRequest<AuthorizeResponseDto>
{
    public string Type { get; set; }

    public string Email { get; set; }

    public string Password { get; set; }

    public string RefreshToken { get; set; }
}

public class AuthorizeCmdHandler
    : IRequestHandler<AuthorizeCmd, AuthorizeResponseDto>
{
    private readonly IStringLocalizer<AccountsResource> _accountLocalizer;

    private readonly AuthOptions _authOptions;

    private readonly IAppDbContext _context;

    private readonly IDateTimeService _dateTimeService;

    private readonly IMediator _mediator;

    private readonly UserManager<AppUser> _userManager;

    public AuthorizeCmdHandler(IOptions<AuthOptions> authOptions
        , IStringLocalizer<AccountsResource> accountLocalizer
        , UserManager<AppUser> userManager
        , IAppDbContext context
        , IDateTimeService dateTimeService
        , IMediator mediator)
    {
        _accountLocalizer = accountLocalizer;
        _userManager = userManager;
        _context = context;
        _dateTimeService = dateTimeService;
        _mediator = mediator;
        _authOptions = authOptions.Value;
    }

    public async Task<AuthorizeResponseDto> Handle(AuthorizeCmd request
        , CancellationToken cancellationToken)
    {
        return request.Type.ToLower() switch
        {
            Token.TypePassword => await GenerateNewTokenUsingPasswordAsync(
                request),

            Token.TypeRefresh => await GenerateNewTokenUsingRefreshAsync(request),

            _ => throw new BaseInvalidAppStateException("unknown token type")
        };
    }

    private async Task<AuthorizeResponseDto> GenerateNewTokenUsingRefreshAsync(AuthorizeCmd request)
    {
        AppUser user;
        try
        {
            var range = ..request.RefreshToken.IndexOf('-');
            var id = request.RefreshToken[range];
            user = await _userManager.FindByIdAsync(id);

            if (user == null)
            {
                throw new NotFoundException(_accountLocalizer["UnableLogin"]);
            }
        }
        catch
        {
            throw new BadRequestException(
                _accountLocalizer["RefreshTokenInvalid"]);
        }

        var token = await _context.Tokens.FirstOrDefaultAsync(t =>
            t.UserId == user.Id
            && t.Client == _authOptions.Audience
            && t.Value == request.RefreshToken);

        if (token == null || token.ExpiryTimeUtc <= _dateTimeService.NowUtc)
        {
            throw new BadRequestException(
                _accountLocalizer["RefreshTokenInvalid"]);
        }

        var refreshToken =
            await _mediator.Send(new CreateRefreshTokenCmd
            {
                UserId = user.Id,
                OldToken = null
            });

        return await _mediator.Send(new CreateAccessTokenCmd
        {
            User = user,
            RefreshToken = refreshToken
        });
    }

    private async Task<AuthorizeResponseDto> GenerateNewTokenUsingPasswordAsync(
        AuthorizeCmd request)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user == null ||
            !await _userManager.CheckPasswordAsync(user, request.Password))
        {
            throw new NotFoundException(_accountLocalizer["UnableLogin"]);
        }

        var refreshToken =
            await _mediator.Send(new CreateRefreshTokenCmd
            {
                UserId = user.Id,
                OldToken = null
            });

        return await _mediator.Send(new CreateAccessTokenCmd
        {
            User = user,
            RefreshToken = refreshToken
        });
    }
}