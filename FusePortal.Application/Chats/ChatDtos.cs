using Facet;
using FusePortal.Domain.Entities.ChatAggregate;

namespace FusePortal.Application.Chats
{
    [Facet(typeof(Message),
            Include =
            [
                nameof(Message.Id),
                nameof(Message.Text),
                nameof(Message.ChatId),
                nameof(Message.CreatedAt),
                nameof(Message.FromUser),
            ])
    ]
    public partial record MessageDto;


    [Facet(typeof(Chat),
            Include =
            [
                nameof(Chat.Id),
                nameof(Chat.Name),
            ])
    ]
    public partial record ChatDto;


    [Facet(typeof(Chat),
            Include =
            [
                nameof(Chat.Id),
                nameof(Chat.Name),
                nameof(Chat.Messages),
            ],
            NestedFacets =
            [
                typeof(MessageDto),
            ]),
    ]
    public partial record ChatWithMessagesDto;
}
