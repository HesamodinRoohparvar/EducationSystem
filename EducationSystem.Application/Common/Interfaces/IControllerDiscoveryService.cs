using EducationSystem.Application.Common.Models.Contorllers;

namespace EducationSystem.Application.Common.Interfaces
{
    public interface IControllerDiscoveryService
    {
        List<Controller> GetAllControllers();
        List<Controller> GetAllSecuredControllers();
        List<ControllerAction> GetAllSecuredActions();
        List<Controller> GetAllSecuredControllers(string policyName);
        List<ControllerAction> GetAllSecuredActions(string policyName);
        List<Controller> GetAllSecuredControllers(string areaName, string policyName);
        List<ControllerAction> GetAllSecuredActions(string areaName, string policyName);
    }
}
