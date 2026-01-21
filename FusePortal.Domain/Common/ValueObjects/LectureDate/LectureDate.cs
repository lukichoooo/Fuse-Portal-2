using FusePortal.Domain.SeedWork;

namespace FusePortal.Domain.Common.ValueObjects.LectureDate
{
    public sealed class LectureDate : ValueObject
    {
        public DateTime Start { get; init; }
        public DateTime End { get; init; }

        public LectureDate(DateTime start, DateTime end)
        {
            Start = start;
            End = end;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Start;
            yield return End;
        }

        private LectureDate() { }
    }
}
