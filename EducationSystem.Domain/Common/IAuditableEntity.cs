namespace EducationSystem.Domain
{
    public interface IAuditableEntity
    {
        public int CreatedBy { get; set; }
        public int LastModifiedBy { get; set; }
    }
}
