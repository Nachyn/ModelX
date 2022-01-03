using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using ModelX.Domain.Entities;
using ModelX.Logic.Common.AppConfigs.Main;
using ModelX.Logic.Common.Exceptions.Base;
using ModelX.Logic.Common.ExternalServices.Database;
using ModelX.Logic.Common.ExternalServices.DateTimeService;

namespace ModelX.Logic.CQRS.Tokens.Commands.CreateRefreshToken;

public class CreateRefreshTokenCmd : IRequest<Token>
{
    public int UserId { get; set; }

    public Token? OldToken { get; set; }
}

public class CreateRefreshTokenCmdHandler
    : IRequestHandler<CreateRefreshTokenCmd, Token>
{
    private readonly AuthOptions _authOptions;

    private readonly IAppDbContext _context;

    private readonly IDateTimeService _dateTimeService;

    public CreateRefreshTokenCmdHandler(IAppDbContext context
        , IOptions<AuthOptions> authOptions
        , IDateTimeService dateTimeService)
    {
        _context = context;
        _dateTimeService = dateTimeService;
        _authOptions = authOptions.Value;
    }

    public async Task<Token> Handle(CreateRefreshTokenCmd request
        , CancellationToken cancellationToken)
    {
        var userId = request.UserId;
        var oldToken = request.OldToken;

        if (userId < 1)
        {
            throw new BaseArgumentException(nameof(userId));
        }

        var token = oldToken ?? await _context.Tokens.FirstOrDefaultAsync(t =>
                t.UserId == userId
                && t.Client == _authOptions.Audience
            , cancellationToken);

        var expiryTimeUtc = _dateTimeService.NowUtc.AddMinutes(_authOptions
            .ExpiryTimeRefreshTokenMinutes);

        var tokenValue = $"{userId}-{Guid.NewGuid():D}";

        if (token == null)
        {
            var newToken = new Token
            {
                Client = _authOptions.Audience,
                ExpiryTimeUtc = expiryTimeUtc,
                UserId = userId,
                Value = tokenValue
            };

            _context.Tokens.Add(newToken);
            await _context.SaveChangesAsync(cancellationToken);
            return newToken;
        }

        token.ExpiryTimeUtc = expiryTimeUtc;
        token.Value = tokenValue;
        token.UpdatedUtc = _dateTimeService.NowUtc;
        await _context.SaveChangesAsync(cancellationToken);
        return token;
    }
}