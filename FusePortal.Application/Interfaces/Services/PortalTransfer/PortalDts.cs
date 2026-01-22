using Facet;
using FusePortal.Domain.Entities.Academic.SubjectAggregate;

namespace FusePortal.Application.Interfaces.Services.PortalTransfer
{
    [Facet(typeof(Lecturer),
            Include =
            [
                nameof(Lecturer.Name),
            ])]
    public partial record LecturerLLMDto;


    [Facet(typeof(Syllabus),
            Include =
            [
                nameof(Syllabus.Name),
                nameof(Syllabus.Content),
            ])]
    public partial record SyllabusLLMDto;


    [Facet(typeof(Schedule),
            Include =
            [
                nameof(Schedule.LectureDate),
                nameof(Schedule.Location),
                nameof(Schedule.Metadata),
            ])]
    public partial record ScheduleLLMDto;


    [Facet(typeof(Subject),
            Include =
            [
                nameof(Subject.Name),
                nameof(Subject.UserId),
                nameof(Subject.Metadata),

                nameof(Subject.Lecturers),
                nameof(Subject.Schedules),
                nameof(Subject.Syllabuses),
            ],
            NestedFacets =
            [
                typeof(LecturerLLMDto),
                typeof(SyllabusLLMDto),
                typeof(ScheduleLLMDto),
            ])]
    public partial record SubjectLLMDto;
}
