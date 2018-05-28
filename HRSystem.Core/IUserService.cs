using HRSystem.Domain;

namespace HRSystem.Core
{
    public interface IUserService
    {
        void CreateUser(Employee employee);
    }
}