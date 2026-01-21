using Facet;
using FusePortal.Domain.Entities.Academic.SubjectAggregate;

namespace FusePortal.Application.Interfaces.Services.PortalTransfer
{
    [Facet(typeof(Lecturer),
            exclude:
            [
                nameof(Lecturer.Id),
                nameof(Lecturer.SubjectId),
            ])]
    public partial record LecturerLLMDto;


    [Facet(typeof(Syllabus),
            exclude:
            [
                nameof(Syllabus.Id),
                nameof(Syllabus.SubjectId),
            ])]
    public partial record SyllabusLLMDto;


    [Facet(typeof(Schedule),
            exclude:
            [
                nameof(Schedule.Id),
                nameof(Schedule.SubjectId),
            ])]
    public partial record ScheduleLLMDto;


    [Facet(typeof(Subject),
            exclude:
            [
                nameof(Subject.Id),
            ],
            NestedFacets =
            [
                typeof(LecturerLLMDto),
                typeof(SyllabusLLMDto),
                typeof(ScheduleLLMDto),
            ])]
    public partial record SubjectLLMDto;
}
