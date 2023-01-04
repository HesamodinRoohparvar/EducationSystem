using EducationSystem.Domain;
using EducationSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace EducationSystem.Application.Common.Interfaces
{
    public interface IAppDbContext
    {
        DbSet<User> Users { get; }
        DbSet<Role> Roles { get; }
        DbSet<Term> Terms { get; }
        DbSet<Course> Courses { get; }
        DbSet<UserToken> UserTokens { get; }
        DbSet<TermCourse> TermCourses { get; }
        DbSet<AcademicField> AcademicFields { get; }
        DbSet<StudentCourse> StudentCourses { get; }
        DbSet<RolePermission> RolePermissions { get; }
        DbSet<AcademicBranch> AcademicBranches { get; }

        Task<int> SaveChangesAsync();
    }
}
