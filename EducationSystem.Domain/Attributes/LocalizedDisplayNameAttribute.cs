using EducationSystem.Domain.Resources;
using System.ComponentModel;

namespace EducationSystem.Domain.Attributes
{
    public class LocalizedDisplayNameAttribute : DisplayNameAttribute
    {
        private readonly string _resourceName;

        public LocalizedDisplayNameAttribute(string resourceName) : base()
        {
            _resourceName = resourceName;
        }

        public override string DisplayName
        {
            get
            {
                return Resource.ResourceManager.GetString(_resourceName);
            }
        }
    }
}
