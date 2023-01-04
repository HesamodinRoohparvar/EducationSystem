namespace EducationSystem.Domain.Entities
{
    public class Term : ITimeableEntity, IAuditableEntity, ISoftDeleteableEntity
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? LastModifiedAt { get; set; }
        public int CreatedBy { get; set; }
        public int LastModifiedBy { get; set; }
        public bool IsDeleted { get; set; }
        public List<TermCourse> TermCourses { get; set; }
    }
}
