using System.Collections.Generic;
using System.Threading.Tasks;
using HRSystem.Domain.Attributes.Base;

namespace HRSystem.Core
{
    public interface IAttributeService
    {
        Task<IEnumerable<AttributeInfo>> GetAll();
        Task<bool> IsExists(int attributeAttributeInfoId);
        Task<bool> IsExists(string name);
        Task Create(AttributeInfo attribute);
        Task Update(AttributeInfo attribute);
        Task<AttributeInfo> GetById(int id);
    }
}