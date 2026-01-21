using FusePortal.Application.Common;
using FusePortal.Application.Common.SeedWork;
using FusePortal.Application.Interfaces.Auth;
using FusePortal.Application.Interfaces.Services.PortalTransfer;
using FusePortal.Domain.Entities.Academic.SubjectAggregate;

namespace FusePortal.Application.UseCases.Academic.Subjects.Commands.SavePortal
{
    public class SavePortalCommandHandler : BaseCommandHandler<SavePortalCommand>
    {
        private readonly ISubjectRepo _repo;
        private readonly IPortalTransferService _portalTransfer;
        private readonly IIdentityProvider _identity;

        public SavePortalCommandHandler(
                ISubjectRepo repo,
                IPortalTransferService portalTransfer,
                IIdentityProvider identity,
                IUnitOfWork uow) : base(uow)
        {
            _portalTransfer = portalTransfer;
            _repo = repo;
            _identity = identity;
        }

        protected override async Task ExecuteAsync(SavePortalCommand command, CancellationToken ct)
        {
            var userId = _identity.GetCurrentUserId();
            var subjectLLMDtos = await _portalTransfer.SavePortalAsync(command.PortalPageText);
            foreach (var LLMDto in subjectLLMDtos)
            {
                var subject = new Subject(LLMDto.Name, userId, LLMDto.Metadata);

                foreach (var l in LLMDto.Lecturers ?? [])
                    subject.AddLecturer(l.Name);
                foreach (var s in LLMDto.Schedules ?? [])
                    subject.AddSchedule(s.LectureDate, s.Location, s.Metadata);
                foreach (var s in LLMDto.Syllabuses ?? [])
                    subject.AddSyllabus(s.Name, s.Content);

                await _repo.AddAsync(subject);
            }
        }
    }
}
