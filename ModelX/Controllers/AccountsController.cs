using Microsoft.AspNetCore.Mvc;
using ModelX.Logic.CQRS.Accounts.Commands.Authorize;
using ModelX.Logic.CQRS.Accounts.Commands.CreateAccount;

namespace ModelX.Controllers;

[Route("api/accounts")]
public class AccountsController : ApiController
{
    [HttpPost]
    public async Task<CreateAccountUserInfoDto> CreateAccount(
        [FromBody] CreateAccountCmd cmd)
    {
        return await Mediator.Send(cmd);
    }

    [HttpPost("auth")]
    public async Task<AuthorizeResponseDto> Authorize(
        [FromBody] AuthorizeCmd cmd)
    {
        return await Mediator.Send(cmd);
    }
}