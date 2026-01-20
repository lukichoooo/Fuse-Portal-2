namespace FusePortal.Application.Common.Settings
{
    public class ValidatorSettings
    {
        public int PageSizeMax { get; init; }

        // atuh
        public int PasswordMinLength { get; init; }
        public int PasswordMaxLength { get; init; }
        public int NameMinLength { get; init; }
        public int NameMaxLength { get; init; }

        // address
        public int CityMinLength { get; init; }
        public int CityMaxLength { get; init; }

        // university
        public int UniNameMaxLength { get; init; }
        public int UniNameMinLength { get; init; }

        // exam
        public int ExamAnswerMaxLength { get; init; }

        // chat
        public int ChatNameMaxLength { get; init; }

        public int MessageMaxLength { get; init; }

        public int MaxFilesInRequest { get; init; }
        public int FileNameMaxLength { get; init; }
    }
}
