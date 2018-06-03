using System;
using System.Collections.Generic;
using HRSystem.Domain;

namespace HRSystem.Core
{
    public interface IAccountService
    {
        IEnumerable<Account> GetUsersUpdatedFrom(DateTime from);

        Account GetByLogin(string login);
        
        void Create(Account account);
        
        void Update(Account account);
    }
}