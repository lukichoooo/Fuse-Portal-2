using FluentValidation;

namespace FusePortal.Application.UseCases.Academic.Subjects.Commands.Child.RemoveSyllabusFromSubject
{
    public class RemoveSyllabusFromSubjectCommandValidator : AbstractValidator<RemoveSyllabusFromSubjectCommand>
    {
        public RemoveSyllabusFromSubjectCommandValidator()
        {
            RuleFor(x => x.SubjectId)
                .NotEmpty();

            RuleFor(x => x.SyllabusId)
                .NotEmpty();
        }
    }
}

