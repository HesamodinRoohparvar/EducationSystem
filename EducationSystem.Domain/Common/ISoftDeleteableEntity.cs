namespace EducationSystem.Domain
{
    public interface ISoftDeleteableEntity
    {
        public bool IsDeleted { get; set; }
    }
}
