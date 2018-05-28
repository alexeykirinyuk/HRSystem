using System;
using System.Collections.Generic;
using HRSystem.Domain;

namespace HRSystem.Core
{
    public interface IActiveDirectoryUserRepository
    {
        IEnumerable<SyncUser> GetAll();
        IEnumerable<SyncUser> GetAllModifiedBeetweenDates(DateTime from, DateTime to);
        
        void Create(SyncUser user);
        void Update(SyncUser user);
        void Block(SyncUser user);
    }
}