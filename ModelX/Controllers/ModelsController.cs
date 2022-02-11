using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ModelX.Logic.CQRS.Models.Commands.DeleteModels;
using ModelX.Logic.CQRS.Models.Commands.PutModels;
using ModelX.Logic.CQRS.Models.Queries.GetModels;

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

    [HttpDelete]
    [Authorize]
    public async Task<DeleteModelsResponseDto> DeleteModelsCmd(
        [FromBody] DeleteModelsCmd cmd)
    {
        return await Mediator.Send(cmd);
    }

    [HttpGet]
    [Authorize]
    public async Task<List<GetModelsModelDto>> GetModels()
    {
        return await Mediator.Send(new GetModelsQ());
    }
}