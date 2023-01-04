namespace EducationSystem.Domain
{
    public interface ITimeableEntity
    {
        public DateTime CreatedAt { get; set; }
        public DateTime? LastModifiedAt { get; set;}
    }
}
