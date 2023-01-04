using EducationSystem.Application.Common.Interfaces;
using EducationSystem.Domain;
using EducationSystem.Infrastructure.Persistence.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace EducationSystem.Infrastructure.Persistence.Interceptors
{
    public class TimeableEntitySaveChangesInterceptor : SaveChangesInterceptor
    {
        private readonly IDateTimeService _dateTimeService;

        public TimeableEntitySaveChangesInterceptor(IDateTimeService dateTimeService)
        {
            _dateTimeService = dateTimeService;
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

            foreach(var entry in context.ChangeTracker.Entries<ITimeableEntity>())
            {
                if(entry.State == EntityState.Added)
                {
                    if(entry.Entity.CreatedAt == default)
                    {
                        entry.Entity.CreatedAt = _dateTimeService.Now;
                    }
                }
                
                if(entry.State == EntityState.Added || entry.State == EntityState.Modified || entry.HasChangedOwnedEntities())
                {
                    if(entry.Entity.LastModifiedAt == default)
                    {
                        entry.Entity.LastModifiedAt = _dateTimeService.Now;
                    }
                }
            }
        }
    }
}
