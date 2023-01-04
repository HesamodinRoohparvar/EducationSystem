using EducationSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EducationSystem.Infrastructure.Persistence.Configurations
{
    public class RolePermissionConfigurations : IEntityTypeConfiguration<RolePermission>
    {
        public void Configure(EntityTypeBuilder<RolePermission> builder)
        {
            builder
                .Property(x => x.Id)
                .UseIdentityColumn();

            builder
                .Property(x => x.Area)
                .HasMaxLength(50)
                .IsRequired();

            builder
                .Property(x => x.Action)
                .HasMaxLength(50)
                .IsRequired();

            builder
                .Property(x => x.Controller)
                .HasMaxLength(50)
                .IsRequired();
        }
    }
}
