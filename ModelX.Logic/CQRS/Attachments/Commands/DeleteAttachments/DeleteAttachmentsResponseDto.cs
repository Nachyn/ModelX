namespace ModelX.Logic.CQRS.Attachments.Commands.DeleteAttachments;

public record DeleteAttachmentsResponseDto
{
    public List<int> Ids { get; set; }
}