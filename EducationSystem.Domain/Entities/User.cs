using EducationSystem.Domain.Entities;
using EducationSystem.Domain.Enumerations;

namespace EducationSystem.Domain
{
    public class User : ITimeableEntity, IAuditableEntity, ISoftDeleteableEntity
    {
        public int Id { get; set; }
        public string FirsName { get; set; }
        public string LastName { get; set; }
        public Nationality Nationality { get; set; }
        public string IdentificationCode { get; set; }
        public Religion Religion { get; set; }
        public DateTime BirthDate { get; set; }
        public string MobileNumber { get; set; }
        public string HomeNumber { get; set; }
        public string Address { get; set; } 
        public string PostalCode { get; set; }
        public string Photo { get; set; }
        public string FatherName { get; set; }
        public string? FatherPhoneNumber { get; set; }
        public string? WorkAddress { get; set; }
        public string? WorkPhoneNumber { get; set; }
        public string UserName { get; set; }
        public string PasswordHash { get; set; }
        public string? Email { get; set; }
        public byte AccessFailedCount { get; set; }
        public DateTimeOffset? LockoutEndAt { get; set; }
        public DateTime? GraduationDate { get; set; }
        public DateTime? LastLoginAt { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? LastModifiedAt { get; set; }
        public int LastModifiedBy { get; set; }
        public bool IsDeleted { get; set; }
        public Role Role { get; set; }
        public int RoleId { get; set; }
        public List<TermCourse> TermCourses { get; set; }
        public List<StudentCourse> StudentCourses { get; set; }
        public AcademicField AcademicField { get; set; }
        public int AcademicFieldId { get; set; }
    }
}
