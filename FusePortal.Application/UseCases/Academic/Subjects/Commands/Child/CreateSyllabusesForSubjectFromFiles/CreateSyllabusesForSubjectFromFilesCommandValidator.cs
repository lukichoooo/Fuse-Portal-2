using FluentValidation;
using FusePortal.Application.Common.Settings;
using FusePortal.Application.Common.Validators;
using Microsoft.Extensions.Options;

namespace FusePortal.Application.UseCases.Academic.Subjects.Commands.Child.CreateSyllabusesForSubjectFromFiles
{
    public class CreateSyllabusesForSubjectFromFilesValidator : AbstractValidator<CreateSyllabusesForSubjectFromFilesCommand>
    {
        public CreateSyllabusesForSubjectFromFilesValidator(IOptions<ValidatorSettings> options)
        {
            var config = options.Value;

            RuleFor(x => x.FileUploads)
                .SetValidator(new FileUploadListValidator(options));
        }
    }
}
