namespace EducationSystem.Application.Common.Models.Contorllers
{
    public class ControllerAction
    {
        public string Name { get; set; }
        public bool IsSecured { get; set; }
        public string AreaName { get; set; }
        public string DisplayName { get; set; }
        public string ControllerId { get; set; }
        public string ControllerName { get; set; }
        public List<Attribute> Attributes { get; set; }
        public string Id => $"{ControllerId}:{Name}".ToLower();
    }
}
