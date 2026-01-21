using Facet;
using FusePortal.Domain.Entities.Academic.SubjectAggregate;

namespace FusePortal.Application.UseCases.Academic.Subjects
{
    [Facet(typeof(Lecturer),
    Include =
        [
            nameof(Lecturer.Id),
            nameof(Lecturer.SubjectId),
            nameof(Lecturer.Name),
        ]),
    ]
    public partial record LecturerDto;


    [Facet(typeof(Schedule),
    Include =
        [
            nameof(Schedule.Id),
            nameof(Schedule.SubjectId),
            nameof(Schedule.LectureDate),
            nameof(Schedule.Location),
            nameof(Schedule.Metadata),
        ]),
    ]
    public partial record ScheduleDto;


    [Facet(typeof(Syllabus),
    Include =
        [
            nameof(Syllabus.Id),
            nameof(Syllabus.SubjectId),
            nameof(Syllabus.Name),
            nameof(Syllabus.Content),
        ]),
    ]
    public partial record SyllabusDto;


    [Facet(typeof(Subject),
    Include =
        [
            nameof(Subject.Id),
            nameof(Subject.Name),
            nameof(Subject.Metadata),
            nameof(Subject.UserId),
        ]),
    ]
    public partial record SubjectMiniDto;

    [Facet(typeof(Subject),
    Include =
        [
            nameof(Subject.Id),
            nameof(Subject.Name),
            nameof(Subject.Metadata),
            nameof(Subject.UserId),

            nameof(Subject.Lecturers),
            nameof(Subject.Schedules),
            nameof(Subject.Syllabuses),
        ],
    NestedFacets =
        [
            typeof(LecturerDto),
            typeof(ScheduleDto),
            typeof(SyllabusDto),
        ]),
    ]
    public partial record SubjectDto;
}
