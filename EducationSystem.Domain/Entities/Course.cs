namespace EducationSystem.Domain.Entities
{
    public class Course : ITimeableEntity, IAuditableEntity, ISoftDeleteableEntity
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? LastModifiedAt { get; set; }
        public int CreatedBy { get; set; }
        public int LastModifiedBy { get; set; }
        public bool IsDeleted { get; set; }
        public Course Prerequisite { get; set; }
        public int? PrerequisiteId { get; set; }
        public AcademicField AcademicField { get; set; }
        public int AcademicFieldId { get; set; }
        public List<TermCourse> TermCourses { get; set; }
    }
}
