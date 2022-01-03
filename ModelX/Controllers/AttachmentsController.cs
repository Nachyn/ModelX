using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ModelX.Logic.CQRS.Attachments.Commands.DeleteAttachments;
using ModelX.Logic.CQRS.Attachments.Commands.UploadAttachments;
using ModelX.Logic.CQRS.Attachments.Queries.DownloadAttachment;

namespace ModelX.Controllers;

[Route("api/attachments")]
public class AttachmentsController : ApiController
{
    [HttpPost]
    [Authorize]
    [DisableRequestSizeLimit]
    public async Task<UploadAttachmentsResponseDto> UploadAttachments(
        [FromForm] UploadAttachmentsCmd cmd)
    {
        return await Mediator.Send(cmd);
    }

    [HttpGet("{FileId}")]
    [Authorize]
    public async Task<FileContentResult> UploadAttachments(
        [FromRoute] DownloadAttachmentQ q)
    {
        return await Mediator.Send(q);
    }

    [HttpDelete]
    [Authorize]
    public async Task<DeleteAttachmentsResponseDto> DeleteAttachments(
        [FromBody] DeleteAttachmentsCmd cmd)
    {
        return await Mediator.Send(cmd);
    }
}