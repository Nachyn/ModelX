using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using ModelX.Logic.Common.AppConfigs.Main;

namespace ModelX.Logic.Common.Validators;

public class FileValidator : AbstractValidator<IFormFile>
{
    private readonly IStringLocalizer<CommonValidatorsResource> _commonLocalizer;
    
    private readonly FileSettings _fileSettings;

    public FileValidator(IOptions<FileSettings> fileSettings
        , IStringLocalizer<CommonValidatorsResource> commonLocalizer)
    {
        _fileSettings = fileSettings.Value;
        _commonLocalizer = commonLocalizer;

        RuleFor(v => v)
            .Custom(BeValidUploadedFile);
    }

    public void BeValidUploadedFile(IFormFile? uploadedFile
        , ValidationContext<IFormFile> context)
    {
        if (uploadedFile == null)
        {
            context.AddFailure(_commonLocalizer["FileNull"]);
            return;
        }

        if (string.IsNullOrWhiteSpace(uploadedFile.ContentType)
            || !_fileSettings.MimeContentTypes.Contains(uploadedFile
                .ContentType))
        {
            context.AddFailure(_commonLocalizer["FileErrorType"]);
            return;
        }

        if (uploadedFile.Length > _fileSettings.MaxLengthBytes)
        {
            context.AddFailure(_commonLocalizer["FileMaxLength"
                , _fileSettings.MaxLengthBytes / 1048576]);
        }

        if (uploadedFile.FileName.Length > _fileSettings.FileNameMaxLength)
        {
            context.AddFailure(_commonLocalizer["FileNameMaxLength"
                , _fileSettings.FileNameMaxLength
                , uploadedFile.FileName]);
        }
    }
}