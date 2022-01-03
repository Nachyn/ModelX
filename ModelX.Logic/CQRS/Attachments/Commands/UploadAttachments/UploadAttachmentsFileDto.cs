using ModelX.Domain.Entities;
using ModelX.Logic.Common.Mappings;

namespace ModelX.Logic.CQRS.Attachments.Commands.UploadAttachments;

public record UploadAttachmentsFileDto : IMapFrom<Attachment>
{
    public int Id { get; set; }

    public string Name { get; set; }

    public DateTime LoadedUtc { get; set; }
}