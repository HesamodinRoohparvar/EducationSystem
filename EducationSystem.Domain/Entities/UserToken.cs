using EducationSystem.Domain.Enumerations;

namespace EducationSystem.Domain.Entities
{
    public class UserToken : ITimeableEntity
    {
        public int Id { get; set; }
        public TokenType Type { get; set; }
        public string Value { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? LastModifiedAt { get; set; }
        public DateTime ExpireAt { get; set; }
    }
}
