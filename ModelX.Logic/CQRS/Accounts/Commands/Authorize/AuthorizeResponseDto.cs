using AutoMapper;
using ModelX.Domain.Entities;
using ModelX.Logic.Common.Mappings;

namespace ModelX.Logic.CQRS.Accounts.Commands.Authorize;

public record AuthorizeResponseDto : IMapFrom<AppUser>
{
    public string Token { get; set; }

    public DateTime ExpirationTokenUtc { get; set; }

    public string RefreshToken { get; set; }

    public DateTime ExpirationRefreshTokenUtc { get; set; }

    public IEnumerable<string> Roles { get; set; }

    public string Username { get; set; }

    public string Email { get; set; }

    public int UserId { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<AppUser, AuthorizeResponseDto>()
            .ForMember(d => d.Token, opt => opt.Ignore())
            .ForMember(d => d.ExpirationTokenUtc, opt => opt.Ignore())
            .ForMember(d => d.RefreshToken, opt => opt.Ignore())
            .ForMember(d => d.ExpirationRefreshTokenUtc, opt => opt.Ignore())
            .ForMember(d => d.Roles, opt => opt.Ignore())
            .ForMember(d => d.UserId, opt => opt.MapFrom(src => src.Id));
    }
}