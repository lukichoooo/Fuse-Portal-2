using Facet;
using FusePortal.Application.UseCases.Content.Files;
using FusePortal.Domain.Entities.Convo.ChatAggregate;

namespace FusePortal.Application.UseCases.Convo.Chats
{
    [Facet(typeof(Message),
            Include =
            [
                nameof(Message.Id),
                nameof(Message.CountNumber),
                nameof(Message.Text),
                nameof(Message.ChatId),
                nameof(Message.CreatedAt),
                nameof(Message.FromUser),
                nameof(Message.Files),
            ],
            NestedFacets =
            [
                typeof(FileDto),
            ])
    ]
    public partial record MessageDto;

    [Facet(typeof(Message),
            Include =
            [
                nameof(Message.Text),
                nameof(Message.ChatId),
                nameof(Message.CreatedAt),
                nameof(Message.FromUser),
                nameof(Message.Files),
            ],
            NestedFacets =
            [
                typeof(FileDto),
            ])
    ]
    public partial record MessageLLMDto;


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
