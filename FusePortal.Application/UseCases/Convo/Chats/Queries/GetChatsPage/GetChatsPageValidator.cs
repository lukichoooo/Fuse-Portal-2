using FluentValidation;
using FusePortal.Application.Common.Settings;
using Microsoft.Extensions.Options;

namespace FusePortal.Application.UseCases.Convo.Chats.Queries.GetChatsPage
{
    public class GetChatsPageValidator : AbstractValidator<GetChatsPageQuery>
    {
        public GetChatsPageValidator(IOptions<ValidatorSettings> options)
        {
            var config = options.Value;


            RuleFor(x => x.PageSize)
                .GreaterThan(0)
                .LessThanOrEqualTo(config.PageSizeMax);
        }
    }
}
