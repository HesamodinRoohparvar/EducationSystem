namespace EducationSystem.Domain.Entities
{
    public class StudentCourse : ITimeableEntity, IAuditableEntity, ISoftDeleteableEntity
    {
        public int Id { get; set; }
        public float Score { get; set; }
        public string? Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? LastModifiedAt { get; set; }
        public int CreatedBy { get; set; }
        public int LastModifiedBy { get; set; }
        public bool IsDeleted { get; set; }
        public TermCourse TermCourse { get; set; }
        public int TermCourseId { get; set; }
        public User Student { get; set; }
        public int StudentId { get; set; }
    }
}
