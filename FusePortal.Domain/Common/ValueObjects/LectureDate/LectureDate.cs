using FusePortal.Domain.SeedWork;

namespace FusePortal.Domain.Common.ValueObjects.LectureDate
{
    public sealed class LectureDate : ValueObject
    {
        public DateTime Start { get; private set; }
        public DateTime End { get; private set; }

        public LectureDate(DateTime start, DateTime end)
        {
            if (start > end)
                throw new LectureDateDomainException("Start date cannot be greater than end date");

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
