using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using ModelX.Domain.Entities;
using ModelX.Logic.Common.AppConfigs.Main;
using ModelX.Logic.Common.Exceptions.Base;
using ModelX.Logic.Common.ExternalServices.DateTimeService;
using ModelX.Logic.CQRS.Accounts.Commands.Authorize;

namespace ModelX.Logic.CQRS.Tokens.Commands.CreateAccessToken;

public record CreateAccessTokenCmd : IRequest<AuthorizeResponseDto>
{
    public AppUser User { get; set; }

    public Token RefreshToken { get; set; }
}

public class CreateAccessTokenCmdHandler
    : IRequestHandler<CreateAccessTokenCmd, AuthorizeResponseDto>
{
    private readonly AuthOptions _authOptions;

    private readonly IDateTimeService _dateTimeService;

    private readonly IMapper _mapper;
    private readonly UserManager<AppUser> _userManager;

    public CreateAccessTokenCmdHandler(UserManager<AppUser> userManager
        , IDateTimeService dateTimeService
        , IOptions<AuthOptions> authOptions
        , IMapper mapper)
    {
        _userManager = userManager;
        _dateTimeService = dateTimeService;
        _mapper = mapper;
        _authOptions = authOptions.Value;
    }

    public async Task<AuthorizeResponseDto> Handle(CreateAccessTokenCmd request
        , CancellationToken cancellationToken)
    {
        var user = request.User;
        var refreshToken = request.RefreshToken;

        if (user == null || refreshToken == null)
        {
            throw new BaseArgumentException();
        }

        var roles = await _userManager.GetRolesAsync(request.User);
        var tokenHandler = new JwtSecurityTokenHandler();
        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.UserName),
            new(JwtRegisteredClaimNames.Jti
                , Guid.NewGuid().ToString()),
            new(ClaimTypes.Name, user.Id.ToString()),
            new("LoggedOn"
                , _dateTimeService.NowUtc.ToString(CultureInfo.InvariantCulture)),
            new(ClaimTypes.Role, string.Join(", ", roles))
        };

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            SigningCredentials =
                new SigningCredentials(_authOptions.SymmetricSecurityKey,
                    SecurityAlgorithms.HmacSha256Signature),
            Issuer = _authOptions.Issuer,
            Audience = _authOptions.Audience,
            Expires =
                _dateTimeService.NowUtc.AddMinutes(_authOptions.ExpiryTimeTokenMinutes)
        };

        var newToken = tokenHandler.CreateToken(tokenDescriptor);
        var encodedToken = tokenHandler.WriteToken(newToken);

        var authorizeResponse = _mapper.Map<AuthorizeResponseDto>(user);
        authorizeResponse.Token = encodedToken;
        authorizeResponse.ExpirationTokenUtc = newToken.ValidTo;
        authorizeResponse.RefreshToken = refreshToken.Value;
        authorizeResponse.ExpirationRefreshTokenUtc = refreshToken.ExpiryTimeUtc;
        authorizeResponse.Roles = roles;
        return authorizeResponse;
    }
}