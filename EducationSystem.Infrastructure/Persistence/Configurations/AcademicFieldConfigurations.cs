﻿using EducationSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EducationSystem.Infrastructure.Persistence.Configurations
{
    public class AcademicFieldConfigurations : IEntityTypeConfiguration<AcademicField>
    {
        public void Configure(EntityTypeBuilder<AcademicField> builder)
        {
            builder
                .Property(x => x.Id)
                .UseIdentityColumn();

            builder
                .Property(x => x.Title)
                .HasMaxLength(50)
                .IsRequired();

            builder
                .Property(x => x.Description)
                .HasMaxLength(250);

            builder
                .HasQueryFilter(x => !x.IsDeleted);
        }
    }
}
