using FluentValidation;
using FusePortal.Application.Common.Settings;
using Microsoft.Extensions.Options;

namespace FusePortal.Application.UseCases.Convo.Chats.Commands.SendMessage
{
    public class SendMessageCommandValidator : AbstractValidator<SendMessageCommand>
    {
        public SendMessageCommandValidator(IOptions<ValidatorSettings> options)
        {
            var config = options.Value;

            RuleFor(x => x.MessageText)
                .NotEmpty()
                .MaximumLength(config.MessageMaxLength);

            RuleFor(x => x.FileUploads)
                .NotNull()
                .Must(files => files.Count <= config.MaxFilesInRequest)
                .WithMessage($"Too many files. Max allowed is {config.MaxFilesInRequest}.");

            RuleForEach(x => x.FileUploads)
                .Must(f => f.Name.Length <= config.FileNameMaxLength)
                .WithMessage($"File name exceeds {config.FileNameMaxLength} characters.");

        }
    }
}
