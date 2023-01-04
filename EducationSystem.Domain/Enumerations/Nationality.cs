using EducationSystem.Domain.Attributes;
using EducationSystem.Domain.Resources;

namespace EducationSystem.Domain.Enumerations
{
    public enum Nationality
    {
        [LocalizedDescription(nameof(Resource.Iran))]
        Iran = 0,

        [LocalizedDescription(nameof(Resource.Afghanistan))]
        Afghanistan = 1,

        [LocalizedDescription(nameof(Resource.Iraq))]
        Iraq = 2,

        [LocalizedDescription(nameof(Resource.Turkey))]
        Turkey = 3,

        [LocalizedDescription(nameof(Resource.Azerbaijan))]
        Azerbaijan = 4,

        [LocalizedDescription(nameof(Resource.Pakistan))]
        Pakistan = 5
    }
}
