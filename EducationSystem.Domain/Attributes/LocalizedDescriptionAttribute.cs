using EducationSystem.Domain.Resources;
using System.ComponentModel;

namespace EducationSystem.Domain.Attributes
{
    public class LocalizedDescriptionAttribute : DescriptionAttribute
    {
        private readonly string _resourceName;

        public LocalizedDescriptionAttribute(string resourcName) : base()
        {
            _resourceName = resourcName;
        }

        public override string Description
        {
            get
            {
                return Resource.ResourceManager.GetString(_resourceName);
            }
        }
    }
}
