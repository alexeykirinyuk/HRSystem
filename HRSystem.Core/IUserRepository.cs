using System;
using System.Collections.Generic;
using HRSystem.Domain;

namespace HRSystem.Core
{
    public interface IUserRepository
    {
        IEnumerable<User> GetUsersUpdatedFrom(DateTime from);

        User GetByLogin(string login);
        
        void Create(User user);
        
        void Update(User user);
    }
}