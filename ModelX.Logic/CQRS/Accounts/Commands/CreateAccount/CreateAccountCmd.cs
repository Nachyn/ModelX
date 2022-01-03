using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using ModelX.Domain.Entities;
using ModelX.Domain.Enums;
using ModelX.Domain.Helpers;
using ModelX.Logic.Common.Exceptions.Api;
using ModelX.Logic.Common.Exceptions.Base;

namespace ModelX.Logic.CQRS.Accounts.Commands.CreateAccount;

public record CreateAccountCmd : IRequest<CreateAccountUserInfoDto>
{
    public string Email { get; set; }

    public string Password { get; set; }

    public string Username { get; set; }
}

public class CreateAccountCommandHandler
    : IRequestHandler<CreateAccountCmd, CreateAccountUserInfoDto>
{
    private readonly IMapper _mapper;

    private readonly UserManager<AppUser> _userManager;

    public CreateAccountCommandHandler(UserManager<AppUser> userManager
        , IMapper mapper)
    {
        _userManager = userManager;
        _mapper = mapper;
    }

    public async Task<CreateAccountUserInfoDto> Handle(CreateAccountCmd request
        , CancellationToken cancellationToken)
    {
        var user = new AppUser
        {
            Email = request.Email.ToLower(),
            UserName = request.Username
        };

        var createResult = await _userManager.CreateAsync(user, request.Password);
        if (!createResult.Succeeded)
        {
            throw new BadRequestException(createResult);
        }

        var roleResult = await _userManager.AddToRoleAsync(user
            , Roles.User.GetEnumDescription());

        if (!roleResult.Succeeded)
        {
            await _userManager.DeleteAsync(
                await _userManager.FindByEmailAsync(user.Email));

            throw new BaseException("error adding role to user");
        }

        var response = _mapper.Map<CreateAccountUserInfoDto>(user);
        response.Roles = (await _userManager.GetRolesAsync(user)).ToList();
        return response;
    }
}