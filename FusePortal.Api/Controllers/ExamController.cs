using FusePortal.Api.Settings;
using FusePortal.Application.UseCases.Academic.Exams.Commands.GenerateExam;
using FusePortal.Application.UseCases.Academic.Exams.Commands.GradeExam;
using FusePortal.Application.UseCases.Academic.Exams.Queries.GetExamById;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace FusePortal.Api.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class ExamController(
            ISender sender,
            IOptions<ControllerSettings> options) : ControllerBase
    {
        private readonly ControllerSettings _settings = options.Value;
        private readonly ISender _sender = sender;

        [HttpGet("{examId:guid}")]
        public async Task<ActionResult<Guid>> GetExamById(
                [FromRoute] Guid examId
                )
            => Ok(await _sender.Send(new GetExamByIdQuery(examId)));

        [HttpPost("generate/{subjectId:guid}")]
        public async Task<ActionResult<Guid>> GenerateExamForSubject(
                [FromRoute] Guid subjectId
                )
            => Ok(await _sender.Send(new GenerateExamCommand(subjectId)));

        [HttpPost]
        public async Task<ActionResult<Guid>> CheckExamAnswersAsync(
                [FromBody] GradeExamCommand gradeExamCommand
                )
            => Ok(await _sender.Send(gradeExamCommand));
    }
}
