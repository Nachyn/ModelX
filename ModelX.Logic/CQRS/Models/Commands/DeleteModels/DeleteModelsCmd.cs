using MediatR;
using Microsoft.EntityFrameworkCore;
using ModelX.Logic.Common.ExternalServices.Database;
using ModelX.Logic.Common.UserAccessor;
using ModelX.Logic.CQRS.Attachments.Commands.DeleteAttachments;

namespace ModelX.Logic.CQRS.Models.Commands.DeleteModels;

public record DeleteModelsCmd : IRequest<DeleteModelsResponseDto>
{
    public List<int> Ids { get; set; }
}

public class DeleteModelsCmdHandler : IRequestHandler<DeleteModelsCmd, DeleteModelsResponseDto>
{
    private readonly IAppDbContext _context;

    private readonly ISender _sender;

    private readonly IUserAccessor _userAccessor;

    public DeleteModelsCmdHandler(ISender sender
        , IAppDbContext context
        , IUserAccessor userAccessor)
    {
        _sender = sender;
        _context = context;
        _userAccessor = userAccessor;
    }

    public async Task<DeleteModelsResponseDto> Handle(DeleteModelsCmd request
        , CancellationToken cancellationToken)
    {
        var models = await _context.Models
            .Where(m => _userAccessor.UserId == m.Attachment.OwnerId
                        && request.Ids.Contains(m.AttachmentId))
            .ToListAsync(CancellationToken.None);

        _context.Models.RemoveRange(models);
        await _context.SaveChangesAsync(CancellationToken.None);

        var removedModelIds = models.Select(m => m.AttachmentId).ToList();
        if (removedModelIds.Any())
        {
            await _sender.Send(new DeleteAttachmentsCmd
            {
                Ids = removedModelIds
            });
        }

        return new DeleteModelsResponseDto
        {
            Ids = removedModelIds
        };
    }
}