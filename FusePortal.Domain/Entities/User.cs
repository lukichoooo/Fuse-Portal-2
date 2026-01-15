using FusePortal.Domain.SeedWork;

namespace FusePortal.Domain.Entities
{
    public class User : Entity
    {
        public string Name { get; private set; }

        public string Email { get; private set; }

        public string Password { get; private set; }

        // public RoleType Role { get; set; } = RoleType.Student;
        //
        // public required Address Address { get; set; } = null!;
        //
        //
        // public List<ChatFile> ChatFiles { get; set; } = [];
        // public List<UserUniversity> UserUniversities { get; set; } = [];
        // public List<Subject> Subjects { get; set; } = [];
        //
        // public List<Chat> Chats { get; set; } = [];
    }
}
