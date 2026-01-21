using FluentValidation;
using FusePortal.Application.Common.Settings;
using FusePortal.Application.Interfaces.Services.File;
using Microsoft.Extensions.Options;

namespace FusePortal.Application.Common.Validators
{
    public class FileUploadListValidator : AbstractValidator<List<FileUpload>>
    {

        public FileUploadListValidator(IOptions<ValidatorSettings> options)
        {
            var config = options.Value;


            RuleFor(x => x)
                .NotNull()
                .Must(files => files.Count <= config.MaxFilesInRequest)
                .WithMessage($"Too many files. Max allowed is {config.MaxFilesInRequest}.");

            RuleForEach(x => x)
                .Must(f => f.Name.Length <= config.FileNameMaxLength)
                .WithMessage($"File name exceeds {config.FileNameMaxLength} characters.");
        }

    }
}
