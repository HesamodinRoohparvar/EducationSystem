using EducationSystem.Application.Common.Interfaces;
using EducationSystem.Domain;
using EducationSystem.Infrastructure.Persistence.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace EducationSystem.Infrastructure.Persistence.Interceptors
{
    public class AuditableEntitySaveChangesInterceptor : SaveChangesInterceptor
    {
        private readonly ICurrentUserService _currentUserService;

        public AuditableEntitySaveChangesInterceptor(ICurrentUserService currentUserService)
        {
            _currentUserService = currentUserService;
        }

        public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
        {
            UpdateEntities(eventData.Context);

            return base.SavingChanges(eventData, result);
        }

        public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken)
        {
            UpdateEntities(eventData.Context);
            return base.SavingChangesAsync(eventData, result, cancellationToken);
        }

        public void UpdateEntities(DbContext context)
        {
            if(context == null)
            {
                return;
            }

            foreach(var entry in context.ChangeTracker.Entries<IAuditableEntity>())
            {
                if(entry.State == EntityState.Added)
                {
                    if(entry.Entity.CreatedBy == default)
                    {
                        entry.Entity.CreatedBy = _currentUserService.UserId;
                    }
                }

                if(entry.State == EntityState.Added || entry.State == EntityState.Modified || entry.HasChangedOwnedEntities())
                {
                    if(entry.Entity.LastModifiedBy == default)
                    {
                        entry.Entity.LastModifiedBy = _currentUserService.UserId;
                    }
                }
            }
        }
    }
}
