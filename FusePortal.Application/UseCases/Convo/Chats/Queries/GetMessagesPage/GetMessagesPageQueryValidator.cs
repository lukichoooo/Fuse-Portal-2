using FluentValidation;
using FusePortal.Application.Common.Settings;
using Microsoft.Extensions.Options;

namespace FusePortal.Application.UseCases.Convo.Chats.Queries.GetMessagesPage
{
    public class GetMessagesPageQueryValidator : AbstractValidator<GetMessagesPageQuery>
    {
        public GetMessagesPageQueryValidator(IOptions<ValidatorSettings> options)
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
