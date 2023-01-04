using EducationSystem.Domain.Attributes;
using EducationSystem.Domain.Resources;

namespace EducationSystem.Domain.Enumerations
{
    public enum Religion
    {
        [LocalizedDescription(nameof(Resource.Islam))]
        Islam = 0,

        [LocalizedDescription(nameof(Resource.Christian))]
        Christian = 1,

        [LocalizedDescription(nameof(Resource.Zarathustra))]
        Zarathustra = 2
    }
}
