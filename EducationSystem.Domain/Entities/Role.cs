namespace EducationSystem.Domain.Entities
{
    public class Role : ITimeableEntity, IAuditableEntity
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? LastModifiedAt { get; set; }
        public int LastModifiedBy { get; set; }
        public List<User> Users { get; set; }
        public List<RolePermission> RolePermissions { get; set; }
    }
}
