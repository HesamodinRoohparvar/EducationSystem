using System.Reflection;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.CompilerServices;
using EducationSystem.Domain.Attributes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using EducationSystem.Application.Common.Interfaces;
using EducationSystem.Application.Common.Models.Contorllers;

namespace EducationSystem.Infrastructure.Services
{
    public class ControllerDiscoveryService : IControllerDiscoveryService
    {
        private List<Controller> _controllers;
        private List<Controller> _securedController;
        private IActionDescriptorCollectionProvider _discoveryProvider;

        public ControllerDiscoveryService(IActionDescriptorCollectionProvider discoveryProvider)
        {
            _discoveryProvider = discoveryProvider;

            GetAllControllers();
            GetAllSecuredControllers();
        }

        public List<Controller> GetAllControllers()
        {
            if (_controllers != null)
            {
                return _controllers;
            }

            _controllers = new();
            var lastControllerName = string.Empty;
            Controller currentController = null;
            var actionDescriptors = _discoveryProvider.ActionDescriptors.Items;

            foreach (var actionDescriptor in actionDescriptors)
            {
                if (!(actionDescriptor is ControllerActionDescriptor descriptor))
                {
                    continue;
                }

                var actionMethodInfo = descriptor.MethodInfo;
                var controllerTypeInfo = descriptor.ControllerTypeInfo;

                if (lastControllerName != descriptor.ControllerName)
                {
                    currentController = new Controller
                    {
                        Actions = new List<ControllerAction>(),
                        Name = descriptor.ControllerName.ToLower(),
                        Attributes = GetAttributes(actionMethodInfo),
                        IsSecured = IsSecured(controllerTypeInfo, actionMethodInfo),
                        AreaName = controllerTypeInfo.GetCustomAttribute<AreaAttribute>()?.RouteValue.ToLower(),
                        DisplayName = controllerTypeInfo.GetCustomAttribute<LocalizedDisplayNameAttribute>()?.DisplayName ?? descriptor.ControllerName,
                    };

                    _controllers.Add(currentController);

                    lastControllerName = descriptor.ControllerName;
                }

                var currentAction = new ControllerAction
                {
                    ControllerId = currentController.Id,
                    AreaName = currentController.AreaName,
                    Name = descriptor.ActionName.ToLower(),
                    ControllerName = currentController.Name,
                    Attributes = GetAttributes(actionMethodInfo),
                    IsSecured = IsSecured(controllerTypeInfo, actionMethodInfo),
                    DisplayName = actionMethodInfo.GetCustomAttribute<LocalizedDisplayNameAttribute>()?.DisplayName ?? descriptor.ActionName,
                };

                if (currentController != null && !currentController.Actions.Any(x => x.Id == currentAction.Id))
                {
                    currentController.Actions.Add(currentAction);
                }
            }

            return _controllers.Where(x => x.Actions.Any()).ToList();
        }

        public List<Controller> GetAllSecuredControllers()
        {
            if (_securedController != null)
            {
                return _securedController;
            }

            _securedController = new();

            foreach (var controller in _controllers)
            {
                if (controller.IsSecured)
                {
                    controller.Actions = controller.Actions.Where(x => x.IsSecured).ToList();

                    _securedController.Add(controller);
                }
            }

            return _securedController;
        }

        public List<ControllerAction> GetAllSecuredActions()
        {
            var result = GetAllSecuredControllers()
                .SelectMany(x => x.Actions)
                .ToList();

            return result;
        }

        public List<Controller> GetAllSecuredControllers(string policyName)
        {
            var result = new List<Controller>();

            foreach (var controller in _controllers.Where(x => x.IsSecured))
            {
                controller.Actions = controller.Actions
                    .Where(x =>
                                x.Attributes.OfType<AuthorizeAttribute>().Any(x => string.Equals(x.Policy, policyName, StringComparison.OrdinalIgnoreCase)) ||
                                controller.Attributes.OfType<AuthorizeAttribute>().Any(x => string.Equals(x.Policy, policyName, StringComparison.OrdinalIgnoreCase)))
                    .ToList();

                result.Add(controller);
            }

            return result.Where(x => x.Actions.Any()).ToList();
        }

        public List<ControllerAction> GetAllSecuredActions(string policyName)
        {
            var result = GetAllSecuredControllers(policyName)
                .SelectMany(x => x.Actions)
                .ToList();

            return result;
        }

        public List<Controller> GetAllSecuredControllers(string areaName, string policyName)
        {
            var result = GetAllSecuredControllers(policyName)
                .Where(x => string.Equals(x.AreaName, areaName, StringComparison.OrdinalIgnoreCase))
                .ToList();

            return result;
        }

        public List<ControllerAction> GetAllSecuredActions(string areaName, string PolicyName)
        {
            var result = GetAllSecuredControllers(areaName, PolicyName)
                .SelectMany(x => x.Actions)
                .ToList();

            return result;
        }

        private List<Attribute> GetAttributes(MemberInfo actionMemberInfo)
        {
            var attributes = actionMemberInfo
                .GetCustomAttributes(inherit: true)
                .Where(x =>
                {
                    var attributeNamespace = x.GetType().Namespace;

                    var result = attributeNamespace != typeof(CompilerGeneratedAttribute).Namespace &&
                                 attributeNamespace != typeof(DebuggerStepThroughAttribute).Namespace;

                    return result;
                });

            return attributes.Cast<Attribute>().ToList();
        }

        private bool IsSecured(MemberInfo controllerTypeInfo, MemberInfo actionMethodeInfo)
        {
            var controllerAllowAnonymousAttribute = controllerTypeInfo.GetCustomAttributes<AllowAnonymousAttribute>(inherit: true);

            if (controllerAllowAnonymousAttribute?.Any() ?? false)
            {
                return false;
            }

            var actionAllowAnonymousAttribute = actionMethodeInfo.GetCustomAttributes<AllowAnonymousAttribute>(inherit: true);

            if (actionAllowAnonymousAttribute?.Any() ?? false)
            {
                return false;
            }

            var controllerAuthorizeAttribute = controllerTypeInfo.GetCustomAttributes<AuthorizeAttribute>(inherit: true);

            if (controllerAuthorizeAttribute?.Any() ?? false)
            {
                return false;
            }

            var actionAuthorizeAttribute = actionMethodeInfo.GetCustomAttributes<AuthorizeAttribute>(inherit: true);

            if (actionAuthorizeAttribute?.Any() ?? false)
            {
                return false;
            }

            return false;
        }
    }
}
