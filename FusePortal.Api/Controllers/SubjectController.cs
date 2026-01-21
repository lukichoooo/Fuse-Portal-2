using System.Text;
using FusePortal.Api.Controllers.Extensions;
using FusePortal.Api.Settings;
using FusePortal.Application.UseCases.Academic.Subjects;
using FusePortal.Application.UseCases.Academic.Subjects.Commands.Child.CreateLecturerForSubject;
using FusePortal.Application.UseCases.Academic.Subjects.Commands.Child.CreateScheduleForSubject;
using FusePortal.Application.UseCases.Academic.Subjects.Commands.Child.CreateSyllabusesForSubjectFromFiles;
using FusePortal.Application.UseCases.Academic.Subjects.Commands.Child.CreateSyllabusForSubject;
using FusePortal.Application.UseCases.Academic.Subjects.Commands.Child.RemoveLecturerFromSubjectCommand;
using FusePortal.Application.UseCases.Academic.Subjects.Commands.Child.RemoveScheduleFromSubjectCommand;
using FusePortal.Application.UseCases.Academic.Subjects.Commands.Child.RemoveSyllabusFromSubject;
using FusePortal.Application.UseCases.Academic.Subjects.Commands.CreateSubject;
using FusePortal.Application.UseCases.Academic.Subjects.Commands.RemoveSubject;
using FusePortal.Application.UseCases.Academic.Subjects.Queries.GetSubjectById;
using FusePortal.Application.UseCases.Academic.Subjects.Queries.GetSubjectsPage;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace FusePortal.Api.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class SubjectController(
            ISender sender,
            IOptions<ControllerSettings> options) : ControllerBase
    {
        private readonly ControllerSettings _settings = options.Value;
        private readonly ISender _sender = sender;



        [HttpGet("subjects")]
        public async Task<ActionResult<List<SubjectDto>>> GetSubjectsPageAsync(
                [FromQuery] Guid? lastSubjectId,
                [FromQuery] int? pageSize)
            => Ok(await _sender.Send(new GetSubjectsPageQuery(
                        lastSubjectId,
                        pageSize ?? _settings.DefaultPageSize)));


        [HttpGet("{subjectId:guid}")]
        public async Task<ActionResult<SubjectDto>> GetSubjectByIdAsync(
                [FromRoute] Guid subjectId)
            => Ok(await _sender.Send(new GetSubjectByIdQuery(subjectId)));


        [HttpPost]
        public async Task<IActionResult> CreateSubjectAsync(
                [FromBody] CreateSubjectCommand command)
            => Ok(await _sender.Send(command));


        [HttpDelete("{subjectId:guid}")]
        public async Task<IActionResult> RemoveSubjectByIdAsync(
                [FromRoute] Guid subjectId)
        {
            await _sender.Send(new RemoveSubjectCommand(subjectId));
            return Ok();
        }


        [HttpPost("schedule")]
        public async Task<IActionResult> AddScheduleAsync(
                [FromBody] CreateScheduleForSubjectCommand command)
        {
            await _sender.Send(command);
            return Ok();
        }


        [HttpDelete("{subjectId:guid}/schedule/{scheduleId:guid}")]
        public async Task<IActionResult> RemoveScheduleAsync(
                [FromRoute] Guid scheduleId,
                [FromRoute] Guid subjectId)
        {
            await _sender.Send(new RemoveScheduleFromSubjectCommand(scheduleId, subjectId));
            return Ok();
        }


        [HttpPost("lecturer")]
        public async Task<IActionResult> AddLecturerAsync(
                [FromBody] CreateLecturerForSubjectCommand command)
        {
            await _sender.Send(command);
            return Ok();
        }


        [HttpDelete("{subjectId:guid}/lecturer/{lecturerId:guid}")]
        public async Task<IActionResult> RemoveLecturerAsync(
                [FromRoute] Guid lecturerId,
                [FromRoute] Guid subjectId)
        {
            await _sender.Send(new RemoveLecturerFromSubjectCommand(lecturerId, subjectId));
            return Ok();
        }


        [HttpPost("syllabus")]
        public async Task<IActionResult> AddSylabusAsync(
                [FromBody] CreateSyllabusForSubjectCommand command)
        {
            await _sender.Send(command);
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

        [HttpPost("file/syllabus/{subjectId:guid}")]
        public async Task<IActionResult> AddSylabusFromFileAsync(
                [FromForm] IFormFileCollection files,
                [FromRoute] Guid subjectId)
        {
            var fileUploads = await files.ToFileUpload();
            await _sender.Send(new CreateSyllabusesForSubjectFromFilesCommand(fileUploads, subjectId));
            return Ok();
        }


        [HttpDelete("{subjectId:guid}/syllabus/{syllabusId}")]
        public async Task<IActionResult> RemoveSylabusAsync(
                [FromRoute] Guid syllabusId,
                [FromRoute] Guid subjectId)
        {
            await _sender.Send(new RemoveSyllabusFromSubjectCommand(syllabusId, subjectId));
            return Ok();
        }

    }

}

