using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ModelX.Logic.CQRS.Models.Commands.PutModels;

namespace ModelX.Controllers;

[Route("api/models")]
public class ModelsController : ApiController
{
    [HttpPut]
    [Authorize]
    public async Task<List<PutModelsModelDto>> PutModels(
        [FromBody] PutModelsCmd cmd)
    {
        return await Mediator.Send(cmd);
    }
}