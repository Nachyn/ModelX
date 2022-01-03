using ModelX.Domain.Entities;
using ModelX.Logic.Common.Mappings;

namespace ModelX.Logic.CQRS.Attachments.Queries.GetAttachments;

public record GetAttachmentsAttachmentDto : IMapFrom<Attachment>
{
    public int Id { get; set; }

    public string Name { get; set; }

    public DateTime LoadedUtc { get; set; }
}