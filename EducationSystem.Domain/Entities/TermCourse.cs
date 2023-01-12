namespace EducationSystem.Domain.Entities
{
    public class TermCourse : ITimeableEntity, IAuditableEntity, ISoftDeleteableEntity
    {
        public int Id { get; set; }
        public TimeSpan Time { get; set; }
        public string? Description { get; set; }
        public DayOfWeek Day { get; set; }
        public Term Term { get; set; }
        public int TermId { get; set; }
        public Course Course { get; set; }
        public int CourseId { get; set; }
        public User Teacher { get; set; }
        public int TeacherId { get; set; }
        public List<StudentCourse> StudentCourses { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? LastModifiedAt { get; set; }
        public int CreatedBy { get; set; }
        public int LastModifiedBy { get; set; }
        public bool IsDeleted { get; set; }
    }
}
