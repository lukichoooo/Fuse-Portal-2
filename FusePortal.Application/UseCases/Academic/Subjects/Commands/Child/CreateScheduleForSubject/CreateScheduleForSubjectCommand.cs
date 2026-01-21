using FusePortal.Domain.Common.ValueObjects.LectureDate;
using MediatR;

namespace FusePortal.Application.UseCases.Academic.Subjects.Commands.Child.CreateScheduleForSubject
{
    public sealed record CreateScheduleForSubjectCommand(
                Guid SubjectId,
                LectureDate LectureDate,
                string? Location = null,
                string? Metadata = null) : IRequest;
}
