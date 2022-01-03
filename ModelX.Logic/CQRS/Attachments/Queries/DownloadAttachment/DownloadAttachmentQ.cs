using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using ModelX.Logic.Common.AppConfigs.Main;
using ModelX.Logic.Common.Exceptions.Api;
using ModelX.Logic.Common.ExternalServices.Database;
using ModelX.Logic.Common.ExternalServices.FileService;
using ModelX.Logic.Common.UserAccessor;

namespace ModelX.Logic.CQRS.Attachments.Queries.DownloadAttachment;

public record DownloadAttachmentQ : IRequest<FileContentResult>
{
    public int FileId { get; set; }
}

public class DownloadAttachmentQHandler : IRequestHandler<DownloadAttachmentQ, FileContentResult>
{
    private readonly IStringLocalizer<AttachmentsResource> _attachmentLocalizer;

    private readonly IAppDbContext _context;

    private readonly IFileService _fileService;

    private readonly RootFileFolderDirectory _rootDirectory;

    private readonly IUserAccessor _userAccessor;

    public DownloadAttachmentQHandler(IAppDbContext context
        , IStringLocalizer<AttachmentsResource> attachmentLocalizer
        , IFileService fileService
        , IOptions<RootFileFolderDirectory> rootDirectory
        , IUserAccessor userAccessor)
    {
        _context = context;
        _attachmentLocalizer = attachmentLocalizer;
        _fileService = fileService;
        _userAccessor = userAccessor;
        _rootDirectory = rootDirectory.Value;
    }

    public async Task<FileContentResult> Handle(DownloadAttachmentQ request
        , CancellationToken cancellationToken)
    {
        var attachment = await _context.Attachments
            .FirstOrDefaultAsync(f => f.Id == request.FileId
                                      && f.OwnerId == _userAccessor.UserId, cancellationToken);

        if (attachment == null)
        {
            throw new NotFoundException(_attachmentLocalizer["FileNotFound"]);
        }

        return _fileService.GetFileFromStorage(
            Path.Combine(_rootDirectory.RootFileFolder, attachment.Path), attachment.Name);
    }
}