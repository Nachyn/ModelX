using FluentValidation;
using Microsoft.Extensions.Localization;
using ModelX.Logic.Common.Validators;

namespace ModelX.Logic.CQRS.Attachments.Commands.UploadAttachments;

public class UploadAttachmentsCmdValidator : AbstractValidator<UploadAttachmentsCmd>
{
    public UploadAttachmentsCmdValidator(FileValidator fileValidator
        , IStringLocalizer<AttachmentsResource> attachmentLocalizer)
    {
        RuleFor(v => v.Files)
            .NotEmpty()
            .WithMessage(attachmentLocalizer["FilesEmpty"]);

        RuleForEach(v => v.Files)
            .SetValidator(fileValidator);
    }
}