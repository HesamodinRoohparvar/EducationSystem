namespace EducationSystem.Application.Common.Models.Contorllers
{
    public class Controller
    {
        public string Name { get; set; }
        public bool IsSecured { get; set; }
        public string AreaName { get; set; }
        public string DisplayName { get; set; }
        public List<ControllerAction> Actions { get; set; }
        public List<Attribute> Attributes { get; set; }
        public string Id => $"{AreaName}:{Name}".ToLower();
    }
}
