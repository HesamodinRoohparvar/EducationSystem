using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using FluentValidation;

namespace EducationSystem.Application
{
    public static class ConfigureServices
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            var executingAssembly = Assembly.GetExecutingAssembly();

            services.AddMediatR(executingAssembly);
            services.AddValidatorsFromAssembly(executingAssembly);

            return services;
        }
    }
}
