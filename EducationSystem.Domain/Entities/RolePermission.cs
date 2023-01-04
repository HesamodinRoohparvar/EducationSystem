namespace EducationSystem.Domain.Entities
{
    public class RolePermission : ITimeableEntity, IAuditableEntity
    {
        public int Id { get; set; }
        public string Area { get; set; }
        public string Action { get; set; }
        public string Controller { get; set; }
        public DateTime CreatedAt { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? LastModifiedAt { get; set; }
        public int LastModifiedBy { get; set; }
        public Role Role { get; set; }
        public int RoleId { get; set; }
    }
}
