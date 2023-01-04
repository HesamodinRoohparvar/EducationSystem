using EducationSystem.Application.Common.Constans;
using EducationSystem.Application.Common.Extensions;
using EducationSystem.Application.Common.Interfaces;
using EducationSystem.Application.Security;
using EducationSystem.Domain;
using EducationSystem.Domain.Enumerations;
using EducationSystem.Domain.Resources;
using EducationSystem.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EducationSystem.Infrastructure.Services
{
    public class DbContextSeedService : IDbContextSeedService
    {
        private readonly AppDbContext _dbContext;
        private readonly ILogger<DbContextSeedService> _logger;
        private readonly IControllerDiscoveryService _controllerDiscoveryService;

        public DbContextSeedService(AppDbContext dbContext, ILogger<DbContextSeedService> logger,
            IControllerDiscoveryService controllerDiscoveryService)
        {
            _dbContext = dbContext;
            _logger = logger;
            _controllerDiscoveryService = controllerDiscoveryService;
        }

        public async Task SeedDataAsync()
        {
            try
            {
                await _dbContext.Database.MigrateAsync();

                await SeedDefaultUserAsync();
            }
            catch(Exception exception)
            {
                _logger.LogError(exception, "an error occurred while seeding database.", exception);
            }
        }

        public async Task SeedDefaultUserAsync()
        {
            var users = new List<User>
            {
                new User
                {
                    FirsName = "علی",
                    LastName = "یاری",
                    UserName = nameof(DefaultRoleName.Admin).ToLower(),
                    PasswordHash = PasswordHasher.Hash(nameof(DefaultRoleName.Admin)),
                    Nationality = Nationality.Iran,
                    IdentificationCode = "0372020202",
                    Religion = Religion.Islam,
                    BirthDate = "1362/04/03".ToDateTime(),
                    MobileNumber = "09123456789",
                    HomeNumber = "02538888468",
                    Address = "خیابان صدوقی - کوچه 33 - پلاک 66",
                    PostalCode = "37126268",
                    Photo = "",
                    FatherName = "عبداله",
                    FatherPhoneNumber = "",
                    WorkAddress = "دانشگاه قم",
                    WorkPhoneNumber = "02532932147",
                    Email = "Aliyari@gmail.com",
                    IsActive = true,
                    Role = new Domain.Entities.Role
                    {
                        Title = DefaultRoleName.Admin,
                        Description = Resource.DefaultRole,
                        RolePermissions = _controllerDiscoveryService
                        .GetAllSecuredActions(AreaName.Admin, PolicyNames.DynamicPermission)
                        .Select(x=> new Domain.Entities.RolePermission
                        {
                            Action = x.Name,
                            Area = x.AreaName,
                            Controller = x.ControllerName
                        }).ToList(),
                    }
                }
            };

            foreach(var item in users)
            {
                if(!_dbContext.Users.Any(x=>x.UserName == item.UserName))
                {
                    _dbContext.Users.Add(item);
                }
            }

            await _dbContext.SaveChangesAsync();
        }
    }
}

