namespace EducationSystem.Domain.Entities
{
    public class AcademicField : ITimeableEntity, IAuditableEntity, ISoftDeleteableEntity
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? LastModifiedAt { get; set; }
        public int CreatedBy { get; set; }
        public int LastModifiedBy { get; set; }
        public bool IsDeleted { get; set; }
        public AcademicBranch AcademicBranch { get; set; }
        public int AcademicBranchId { get; set; }
        public List<Course> Courses { get; set; }
        public User User { get; set; }
        public int UserId { get; set; }
    }
}
