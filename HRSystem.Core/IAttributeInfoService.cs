using System.Collections.Generic;
using System.Threading.Tasks;
using HRSystem.Domain.Attributes.Base;

namespace HRSystem.Core
{
    public interface IAttributeInfoService
    {
        Task<IEnumerable<AttributeInfo>> GetAll();
        
        Task<bool> IsExists(int attributeInfoId);
        
        Task<bool> IsExists(string name);
        
        Task Create(AttributeInfo attribute);
        
        Task Update(AttributeInfo attribute);
        
        Task<AttributeInfo> GetById(int id);
        
        Task Delete(AttributeInfo attributeInfo);
    }
}