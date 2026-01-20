using FusePortal.Api.Controllers.Extensions;
using FusePortal.Api.Settings;
using FusePortal.Application.UseCases.Convo.Chats;
using FusePortal.Application.UseCases.Convo.Chats.Commands.CreateChat;
using FusePortal.Application.UseCases.Convo.Chats.Commands.SendMessage;
using FusePortal.Application.UseCases.Convo.Chats.Queries.GetChatsPage;
using FusePortal.Application.UseCases.Convo.Chats.Queries.GetMessagesPage;
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

        [HttpGet("messages/{chatId:guid}")]
        public async Task<ActionResult<List<MessageDto>>> GetMessages(
                    [FromRoute] Guid chatId,
                    [FromQuery] int? topMsgCountNumber,
                    [FromQuery] int? pageSize)
            => Ok(await _sender.Send(new GetMessagesPageQuery(
                            chatId,
                        topMsgCountNumber,
                        pageSize ?? _settings.BigPageSize)));


        [HttpPost("create/{chatName}")]
        public async Task<IActionResult> CreateChat(
                [FromRoute] string? chatName)
        {
            await _sender.Send(new CreateChatCommand(chatName ?? "New Chat"));
            return Ok();
        }


        // DTO
        public class SendMessageRequest
        {
            [FromForm(Name = "messageText")]
            public string MessageText { get; set; } = null!;

            [FromForm(Name = "files")]
            public IFormFileCollection? Files { get; set; }
        }

        [HttpPost("send/{chatId:guid}")]
        public async Task<IActionResult> SendMessage(
            [FromRoute] Guid chatId,
            [FromForm] SendMessageRequest request)
        {
            var uploads = request.Files != null
                ? await request.Files.ToFileUpload()
                : [];

            await _sender.Send(new SendMessageCommand(
                chatId,
                request.MessageText,
                uploads,
                Streaming: false));

            return Ok();
        }



        [HttpPost("ws/send/{ChatId:guid}")]
        public async Task<IActionResult> SendMessageStreaming(
            [FromRoute] Guid chatId,
            [FromForm] SendMessageRequest request)
        {
            var uploads = request.Files != null
                ? await request.Files.ToFileUpload()
                : [];

            await _sender.Send(new SendMessageCommand(
                chatId,
                request.MessageText,
                uploads,
                Streaming: true));

            return Ok();

        }


    }
}
