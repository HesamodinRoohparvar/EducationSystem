using System.Reflection;
using EducationSystem.Domain;
using Microsoft.EntityFrameworkCore;
using EducationSystem.Domain.Entities;
using EducationSystem.Application.Common.Interfaces;
using EducationSystem.Infrastructure.Persistence.Interceptors;

namespace EducationSystem.Infrastructure.Persistence
{
    public class AppDbContext : DbContext, IAppDbContext
    {
        private readonly AuditableEntitySaveChangesInterceptor _auditableEntityInterceptor;
        private readonly TimeableEntitySaveChangesInterceptor _timeableEntityInterceptor;
        private readonly SoftDeleteableSaveChangesInterceptor _softDeleteableInterceptor;

        public AppDbContext(AuditableEntitySaveChangesInterceptor auditableEntityInterceptor,
            TimeableEntitySaveChangesInterceptor timeableableEntityInterceptor,
            SoftDeleteableSaveChangesInterceptor softDeleteableEntityInterceptor,
            DbContextOptions<AppDbContext> options) : base(options)
        {
            _auditableEntityInterceptor = auditableEntityInterceptor;
            _timeableEntityInterceptor = timeableableEntityInterceptor;
            _softDeleteableInterceptor = softDeleteableEntityInterceptor;
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Term> Terms { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<UserToken> UserTokens { get; set; }
        public DbSet<TermCourse> TermCourses { get; set; }
        public DbSet<AcademicField> AcademicFields { get; set; }
        public DbSet<StudentCourse> StudentCourses { get; set; }
        public DbSet<RolePermission> RolePermissions { get; set; }
        public DbSet<AcademicBranch> AcademicBranches { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            base.OnModelCreating(builder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder builder)
        {
            builder.AddInterceptors(_auditableEntityInterceptor);
            builder.AddInterceptors(_timeableEntityInterceptor);
            builder.AddInterceptors(_timeableEntityInterceptor);

            base.OnConfiguring(builder);
        }

        public async Task<int> SaveChangesAsync()
        {
            return await base.SaveChangesAsync();
        }
    }
}
