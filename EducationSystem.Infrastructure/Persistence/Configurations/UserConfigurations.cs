using EducationSystem.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EducationSystem.Infrastructure.Persistence.Configurations
{
    public class UserConfigurations : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder
                .Property(x => x.Id)
                .UseIdentityColumn();

            builder
                .Property(x => x.FirsName)
                .HasMaxLength(25)
                .IsRequired();

            builder
                .Property(x => x.LastName)
                .HasMaxLength(25)
                .IsRequired();

            builder
                .Property(x => x.UserName)
                .HasMaxLength(11)
                .IsRequired();

            builder
                .Property(x => x.PasswordHash)
                .HasMaxLength(150)
                .IsRequired();

            builder
                .Property(x => x.Address)
                .HasMaxLength(800)
                .IsRequired();

            builder
                .Property(x => x.Email)
                .HasMaxLength(320);

            builder
                .Property(x => x.WorkAddress)
                .HasMaxLength(800);

            builder
                .Property(x => x.MobileNumber)
                .HasMaxLength(11)
                .IsRequired();

            builder
                .Property(x => x.HomeNumber)
                .HasMaxLength(11)
                .IsRequired();

            builder
                .Property(x => x.FatherPhoneNumber)
                .HasMaxLength(11)
                .IsRequired();

            builder
                .Property(x => x.WorkPhoneNumber)
                .HasMaxLength(11);

            builder
                .Property(x => x.IdentificationCode)
                .HasMaxLength(11)
                .IsRequired();

            builder
                .Property(x => x.PostalCode)
                .HasMaxLength(10)
                .IsRequired();

            builder
                .Property(x => x.Photo)
                .HasMaxLength(250)
                .IsRequired();

            builder
                .HasQueryFilter(x => !x.IsDeleted);
        }
    }
}
