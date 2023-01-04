namespace EducationSystem.Application.Common.Interfaces
{
    public interface IUserNameGeneratorService
    {
        string GenerateUserName(int academicFieldId, int userCount);
    }
}
