using FluentValidation;
using FusePortal.Application.Common.Settings;
using Microsoft.Extensions.Options;

namespace FusePortal.Application.UseCases.Convo.Chats.Queries.GetChatWithMessagesPage
{
    public class GetChatsWithMessagesQueryValidator : AbstractValidator<GetChatWithMessagesQuery>
    {
        public GetChatsWithMessagesQueryValidator(IOptions<ValidatorSettings> options)
        {
            var config = options.Value;


            RuleFor(x => x.ChatId)
                .NotNull();

            RuleFor(x => x.PageSize)
                .NotNull()
                .LessThanOrEqualTo(config.PageSizeMax);
        }
    }
}
