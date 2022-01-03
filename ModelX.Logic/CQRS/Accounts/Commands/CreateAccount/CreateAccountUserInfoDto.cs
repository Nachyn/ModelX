using AutoMapper;
using ModelX.Domain.Entities;
using ModelX.Logic.Common.Mappings;

namespace ModelX.Logic.CQRS.Accounts.Commands.CreateAccount;

public record CreateAccountUserInfoDto : IMapFrom<AppUser>
{
    public int Id { get; set; }

    public string Username { get; set; }

    public string Email { get; set; }

    public List<string> Roles { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<AppUser, CreateAccountUserInfoDto>()
            .ForMember(d => d.Roles, opt => opt.Ignore());
    }
}