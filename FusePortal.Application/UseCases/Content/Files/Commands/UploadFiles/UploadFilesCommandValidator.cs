using FluentValidation;
using FusePortal.Application.Common.Settings;
using Microsoft.Extensions.Options;

namespace FusePortal.Application.UseCases.Content.Files.Commands.UploadFiles
{
    public class UploadFilesCommandValidator : AbstractValidator<UploadFilesCommand>
    {
        public UploadFilesCommandValidator(IOptions<ValidatorSettings> options)
        {
            var config = options.Value;

            RuleFor(x => x.FileUploads)
                .NotEmpty()
                .Must(x => x.Count < config.MaxFilesInRequest);
        }
    }
}
