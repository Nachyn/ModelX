using FluentValidation;
using ModelX.Logic.Common.Validators;

namespace ModelX.Logic.CQRS.Attachments.Commands.DeleteAttachments;

public class DeleteAttachmentsCmdValidator : AbstractValidator<DeleteAttachmentsCmd>
{
    public DeleteAttachmentsCmdValidator(IdsCountValidator<DeleteAttachmentsCmd> idsValidator)
    {
        RuleFor(v => v.Ids).SetValidator(idsValidator);
    }
}