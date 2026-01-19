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

            RuleFor(x => x.FileIds)
                .Must(x => x == null
                        || x.Count <= config.MaxFilesInRequest);
        }
    }
}
