using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HRSystem.Common.Errors;
using HRSystem.Core;
using HRSystem.Data;
using HRSystem.Domain.Attributes.Base;
using Microsoft.EntityFrameworkCore;

namespace HRSystem.Bll
{
    public class AttributeService : IAttributeService
    {
        private readonly HrSystemDb _db;

        public AttributeService(HrSystemDb db)
        {
            ArgumentHelper.EnsureNotNull(nameof(db), db);
            
            _db = db;
        }

        public async Task<IEnumerable<AttributeInfo>> GetAll()
        {
            return await _db.AttributeInfos.ToArrayAsync().ConfigureAwait(false);
        }

        public async Task<bool> IsExists(int attributeAttributeInfoId)
        {
            return await _db.AttributeInfos.AnyAsync(a => a.Id == attributeAttributeInfoId).ConfigureAwait(false);
        }

        public async Task<bool> IsExists(string name)
        {
            return await _db.AttributeInfos.AnyAsync(a => a.Name == name).ConfigureAwait(false);
        }

        public async Task Create(AttributeInfo attribute)
        {
            await _db.AttributeInfos.AddAsync(attribute).ConfigureAwait(false);
            await _db.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task Update(AttributeInfo attribute)
        {
            var item = await _db.AttributeInfos.SingleAsync(a => a.Id == attribute.Id).ConfigureAwait(false);
            item.Name = attribute.Name;
            item.Type = attribute.Type;
            
            await _db.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task<AttributeInfo> GetById(int id)
        {
            return await _db.AttributeInfos.SingleAsync(a => a.Id == id).ConfigureAwait(false);
        }
    }
}