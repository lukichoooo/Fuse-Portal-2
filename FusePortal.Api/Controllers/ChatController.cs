using FusePortal.Api.Settings;
using FusePortal.Application.UseCases.Convo.Chats;
using FusePortal.Application.UseCases.Convo.Chats.Commands.CreateChat;
using FusePortal.Application.UseCases.Convo.Chats.Commands.SendMessage;
using FusePortal.Application.UseCases.Convo.Chats.Queries.GetChatsPage;
using FusePortal.Application.UseCases.Convo.Chats.Queries.GetChatWithMessagesPage;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace FusePortal.Api.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class ChatController(
            ISender sender,
            IOptions<ControllerSettings> options) : ControllerBase
    {
        private readonly ControllerSettings _settings = options.Value;
        private readonly ISender _sender = sender;


        [HttpGet("all")]
        public async Task<ActionResult<List<ChatDto>>> GetChats(
                    [FromQuery] Guid? lastId,
                    [FromQuery] int? pageSize)
            => Ok(await _sender.Send(new GetChatsPageQuery(
                        lastId,
                        pageSize ?? _settings.DefaultPageSize)));

        [HttpGet("{chatId:guid}")]
        public async Task<ActionResult<List<ChatDto>>> GetChatWithMessages(
                    [FromRoute] Guid chatId,
                    [FromQuery] Guid? firstMsgId,
                    [FromQuery] int? pageSize)
            => Ok(await _sender.Send(new GetChatWithMessagesQuery(
                            chatId,
                        firstMsgId,
                        pageSize ?? _settings.BigPageSize)));


        [HttpPost("create/{chatName}")]
        public async Task<IActionResult> CreateChat(
                [FromRoute] string? chatName)
        {
            await _sender.Send(new CreateChatCommand(chatName ?? "New Chat"));
            return Ok();
        }

        [HttpPost("send")]
        public async Task<IActionResult> SendMessage(
                [FromBody] SendMessageCommand sendMessageCommand)
        {
            await _sender.Send(sendMessageCommand with { Streaming = false });
            return Ok();
        }


        [HttpPost("ws/send")]
        public async Task<IActionResult> SendMessageStreaming(
                [FromBody] SendMessageCommand sendMessageCommand)
        {
            await _sender.Send(sendMessageCommand with { Streaming = true });
            return Ok();
        }

    }
}
