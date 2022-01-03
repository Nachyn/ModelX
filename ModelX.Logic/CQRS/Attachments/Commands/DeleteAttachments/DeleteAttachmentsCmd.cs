using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using ModelX.Logic.Common.AppConfigs.Main;
using ModelX.Logic.Common.ExternalServices.Database;
using ModelX.Logic.Common.ExternalServices.FileService;
using ModelX.Logic.Common.UserAccessor;

namespace ModelX.Logic.CQRS.Attachments.Commands.DeleteAttachments;

public record DeleteAttachmentsCmd : IRequest<DeleteAttachmentsResponseDto>
{
    public List<int> Ids { get; set; }
}

public class DeleteAttachmentsCmdHandler
    : IRequestHandler<DeleteAttachmentsCmd, DeleteAttachmentsResponseDto>
{
    private readonly IAppDbContext _context;

    private readonly IFileService _fileService;

    private readonly RootFileFolderDirectory _rootDirectory;

    private readonly IUserAccessor _userAccessor;

    public DeleteAttachmentsCmdHandler(IAppDbContext context
        , IUserAccessor userAccessor
        , IFileService fileService
        , IOptions<RootFileFolderDirectory> rootDirectory)
    {
        _context = context;
        _userAccessor = userAccessor;
        _fileService = fileService;
        _rootDirectory = rootDirectory.Value;
    }

    public async Task<DeleteAttachmentsResponseDto> Handle(DeleteAttachmentsCmd request
        , CancellationToken cancellationToken)
    {
        var foundAttachments = await _context.Attachments
            .Where(a => a.OwnerId == _userAccessor.UserId
                        && request.Ids.Contains(a.Id))
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        _fileService.DeleteFilesFromStorage(_rootDirectory.RootFileFolder
            , foundAttachments.Select(p => p.Path).ToArray());

        _context.Attachments.RemoveRange(foundAttachments);
        await _context.SaveChangesAsync(cancellationToken);

        return new DeleteAttachmentsResponseDto
        {
            Ids = foundAttachments.Select(p => p.Id).ToList()
        };
    }
}