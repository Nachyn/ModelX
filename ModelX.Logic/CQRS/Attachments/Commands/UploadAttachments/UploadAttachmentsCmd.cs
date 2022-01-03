using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using ModelX.Domain.Entities;
using ModelX.Logic.Common.AppConfigs.Main;
using ModelX.Logic.Common.ExternalServices.Database;
using ModelX.Logic.Common.ExternalServices.DateTimeService;
using ModelX.Logic.Common.ExternalServices.FileService;
using ModelX.Logic.Common.UserAccessor;

namespace ModelX.Logic.CQRS.Attachments.Commands.UploadAttachments;

public record UploadAttachmentsCmd : IRequest<UploadAttachmentsResponseDto>
{
    public List<IFormFile> Files { get; set; }
}

public class UploadAttachmentsCmdHandler
    : IRequestHandler<UploadAttachmentsCmd, UploadAttachmentsResponseDto>
{
    private readonly IAppDbContext _context;

    private readonly IDateTimeService _dateTimeService;

    private readonly IFileService _fileService;

    private readonly IMapper _mapper;

    private readonly RootFileFolderDirectory _rootDirectory;

    private readonly IUserAccessor _userAccessor;

    public UploadAttachmentsCmdHandler(IDateTimeService dateTimeService
        , IOptions<RootFileFolderDirectory> rootDirectory
        , IFileService fileService
        , IUserAccessor userAccessor
        , IAppDbContext context
        , IMapper mapper)
    {
        _dateTimeService = dateTimeService;
        _rootDirectory = rootDirectory.Value;
        _fileService = fileService;
        _userAccessor = userAccessor;
        _context = context;
        _mapper = mapper;
    }

    public async Task<UploadAttachmentsResponseDto> Handle(UploadAttachmentsCmd request
        , CancellationToken cancellationToken)
    {
        var newAttachments = new List<Attachment>();
        foreach (var uploadedFile in request.Files)
        {
            var loadedUtc = _dateTimeService.NowUtc;
            var filePath =
                $"{loadedUtc:yyyyMMddHHmmss}_{Guid.NewGuid():N}_{uploadedFile.FileName}";

            var fullPath = Path.Combine(_rootDirectory.RootFileFolder, filePath);

            await _fileService.WriteToStorageAsync(uploadedFile, fullPath);

            newAttachments.Add(new Attachment
            {
                OwnerId = _userAccessor.UserId,
                Name = uploadedFile.FileName,
                Path = filePath,
                LoadedUtc = loadedUtc
            });
        }

        _context.Attachments.AddRange(newAttachments);
        await _context.SaveChangesAsync(cancellationToken);

        return new UploadAttachmentsResponseDto
        {
            Files = _mapper.Map<List<UploadAttachmentsFileDto>>(newAttachments)
        };
    }
}